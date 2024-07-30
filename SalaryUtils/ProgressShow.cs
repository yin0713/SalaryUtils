namespace SalaryUtils
{
    public class ProgressShow
    {
        public static int Index { get; set; } = 0;
        public static int Total { get; set; } = -1;
        public static string Desc { get; set; } = "";

        private static readonly object obj = new();

        public static void Show(int total = -1, int step = 100, string desc = "")
        {
            lock (obj)
            {
                Total = total;
                Desc = desc;
                if (Index % step == 0)
                {
                    string entry = $"{(Desc == "" ? "" : Desc + ": ")}" + Index + $"{(Total == -1 ? string.Empty : $" | {(double)Index / Total:P} | {Total}")}";
                    Console.Write(new string('\b', entry.Length) + entry);
                }
                Index++;
            }
        }

        public static void LastShow()
        {
            lock (obj)
            {
                if (Index != 0)
                {
                    string entry = $"{(Desc == "" ? "" : Desc + ": ")}" + Index + $"{(Total == -1 ? string.Empty : $" | {(double)Index / Total:P} | {Total}")}";
                    Console.Write(new string('\b', entry.Length) + entry + Environment.NewLine);
                }

                Index = 0;
                Total = -1;
                Desc = "";
            }
        }
    }
}
