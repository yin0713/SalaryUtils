namespace SalaryUtils
{
    public class Node(string word)
    {
        public string word = word;
        public string words = "";
        public Dictionary<string, Node> Children = [];
        public Node? Fail = null;
        public List<string> words_list = [];
    }

    public class ACAutomaton
    {
        public Node root = new("");
        public bool IgnoreCases = false;

        public void Build(string[] patterns, bool ignoreCases = false)
        {
            IgnoreCases = ignoreCases;
            BuildTriTree(patterns);
            BuildFailPointers();
            GenerateWordsList();
        }

        public void Build(string[][] patterns, bool ignoreCases = false)
        {
            IgnoreCases = ignoreCases;
            BuildTriTree(patterns);
            BuildFailPointers();
            GenerateWordsList();
        }

        public void BuildTriTree(string[] patterns)
        {
            if (IgnoreCases)
            {
                patterns = patterns.Select(x => x.ToLower()).ToArray();
            }
            foreach (string pattern in patterns)
            {
                var cur = root;
                foreach (var ch in pattern)
                {
                    if (!cur.Children.ContainsKey(ch.ToString()))
                        cur.Children.Add(ch.ToString(), new Node(ch.ToString()));
                    cur = cur.Children[ch.ToString()];
                }
                cur.words = pattern;
            }
        }

        public void BuildTriTree(string[][] patterns)
        {
            if (IgnoreCases)
            {
                patterns = patterns.Select(x => x.Select(y => y.ToLower()).ToArray()).ToArray();
            }
            foreach (var pattern in patterns)
            {
                var cur = root;
                foreach (var ch in pattern)
                {
                    if (!cur.Children.ContainsKey(ch.ToString()))
                        cur.Children.Add(ch.ToString(), new Node(ch.ToString()));
                    cur = cur.Children[ch.ToString()];
                }
                cur.words = string.Join(" ", pattern);
            }
        }

        public void BuildFailPointers()
        {
            var queue = new Queue<Node>();
            root.Fail = root;
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var ch in cur.Children.Keys)
                {
                    if (cur != root && cur.Fail.Children.ContainsKey(ch))
                        cur.Children[ch].Fail = cur.Fail.Children[ch];
                    else
                        cur.Children[ch].Fail = root;

                    queue.Enqueue(cur.Children[ch]);
                }
            }
        }

        public void GenerateWordsList()
        {
            var queue = new Queue<Node>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var ch in cur.Children.Keys)
                {
                    var tmp = cur.Children[ch];
                    while (tmp != root)
                    {
                        if (tmp.words != "")
                            cur.Children[ch].words_list.Add(tmp.words);
                        tmp = tmp.Fail;
                    }
                    queue.Enqueue(cur.Children[ch]);
                }
            }
        }

        public List<(int start, int end, string text)> Search(string text)
        {
            var orginalText = text;
            if (IgnoreCases)
            {
                text = text.ToLower();
            }
            var result = new List<(int, int, string)>();
            var cur = root;
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                while (!cur.Children.ContainsKey(ch.ToString()) && cur != root)
                    cur = cur.Fail;
                if (cur.Children.ContainsKey(ch.ToString()))
                {
                    var words_list = cur.Children[ch.ToString()].words_list;
                    foreach (var words in words_list)
                    {
                        result.Add(((i + 1 - words.Length), (i + 1), orginalText[(i + 1 - words.Length)..(i + 1)]));
                    }

                    cur = cur.Children[ch.ToString()];
                }
            }
            return result;
        }

        public List<(int start, int end, string[] text)> Search(string[] text)
        {
            var orginalText = text;
            if (IgnoreCases)
            {
                text = text.Select(x => x.ToLower()).ToArray();
            }
            var result = new List<(int, int, string[])>();
            var cur = root;
            for (var i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                while (!cur.Children.ContainsKey(ch.ToString()) && cur != root)
                    cur = cur.Fail;
                if (cur.Children.ContainsKey(ch.ToString()))
                {
                    var words_list = cur.Children[ch.ToString()].words_list;
                    foreach (var words in words_list)
                    {
                        result.Add(((i + 1 - words.Split().Length), (i + 1), orginalText[(i + 1 - words.Split().Length)..(i + 1)]));
                    }

                    cur = cur.Children[ch.ToString()];
                }
            }
            return result;
        }
    }
}
