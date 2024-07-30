using System.Text.RegularExpressions;

namespace SalaryUtils
{
    public class BM25
    {
        private readonly double k1;
        private readonly double b;
        private readonly Regex re_tokenize;
        private readonly Dictionary<int, string> id2doc = [];
        private readonly Dictionary<int, string> id2lowerdoc = [];
        private readonly Dictionary<int, string[]> id2tokens = [];
        private readonly Dictionary<int, int> id2doclength = [];
        private readonly Dictionary<int, Dictionary<string, int>> id2token_count = [];
        private readonly string[] vocabulary;
        private readonly Dictionary<string, double> token2idf = [];
        private readonly int all_docs_num;
        private readonly double all_docs_avg_length;

        public BM25(string docs_file, double k1 = 1.5, double b = 0.75, bool IgnoreCase = true)
        {
            this.k1 = k1;
            this.b = b;
            var token_pattern = @"\w+";
            re_tokenize = new Regex(token_pattern);

            var all_lines = File.ReadAllLines(docs_file).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToArray();
            var all_tokens = new List<string>();
            for (var i = 0; i < all_lines.Length; i++)
            {
                var line = all_lines[i];
                id2doc[i] = line;
                line = line.ToLower();
                id2lowerdoc[i] = line;
                var tokens = Tokenize(line);
                id2tokens[i] = tokens;
                id2doclength[i] = tokens.Length;
                all_tokens.AddRange(tokens);
                id2token_count[i] = tokens.GroupBy(x => x, (key, elements) => (key, elements.Count())).ToDictionary(x => x.key, x => x.Item2);
            }
            all_docs_num = id2doc.Count;
            all_docs_avg_length = id2doclength.Sum(x => x.Value) / (double)all_docs_num;
            vocabulary = all_tokens.Distinct().ToArray();
            foreach (var token in vocabulary)
            {
                var doc_count = 0;
                foreach (var kv in id2token_count)
                {
                    if (kv.Value.ContainsKey(token))
                        doc_count++;
                }
                //token2idf[token] = Math.Log((all_docs_num - doc_count + 0.5) / (doc_count + 0.5));
                token2idf[token] = Math.Log((all_docs_num + 0.5) / (doc_count + 0.5));
            }
        }

        public (double score, string doc)[] Search(string query, int top_k = 10)
        {
            var query_tokens = Tokenize(query).Where(x => x.Length > 0).Select(x => x.ToLower()).Distinct().ToList();
            var id2score = new Dictionary<int, double>();
            foreach (var kv in id2token_count)
            {
                var score_sum = 0.0;
                foreach (var token in query_tokens)
                {
                    if (vocabulary.Contains(token) && kv.Value.TryGetValue(token, out int value))
                    {
                        var score = token2idf[token] * (value * (k1 + 1) / (value + k1 * (1 - b + b * id2doclength[kv.Key] / all_docs_avg_length)));
                        score_sum += score;
                    }
                }
                id2score[kv.Key] = score_sum;
            }

            return id2score.Where(kv => kv.Value > 0).OrderByDescending(kv => kv.Value).Take(top_k).Select(x => (x.Value, id2doc[x.Key])).ToArray();
        }

        public string[] Tokenize(string text)
        {
            /*
             If our regexp matches gaps, use re.split, If our regexp matches tokens, use re.findall.
             tokenize or span_tokenize | gap=true or false
             RegexpTokenizer:\w+|\$[\d\.]+|\S+
             WhitespaceTokenizer:\s+
             BlanklineTokenizer:\s*\n\s*\n\s*
             WordPunctTokenizer:\w+|[^\w\s]+
             */
            return re_tokenize.Matches(text).Select(x => x.Value).ToArray();
        }
    }
}
