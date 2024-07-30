namespace SalaryUtils
{
    public class EditDistance
    {
        public static int GetEditDistance(string text1, string text2)
        {
            var dp = new int[text1.Length + 1, text2.Length + 1];
            for (int i = 0; i <= text1.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= text1.Length; j++)
                dp[0, j] = j;
            for (int i = 1; i <= text2.Length; i++)
            {
                for (int j = 1; j <= text2.Length; j++)
                {
                    if (text1[i - 1] != text2[j - 1])
                    {
                        dp[i, j] = new int[] { dp[i - 1, j - 1], dp[i - 1, j], dp[i, j - 1] }.Min() + 1;
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                }
            }
            return dp[text1.Length, text2.Length];
        }
    }
}
