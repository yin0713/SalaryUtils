namespace SalaryUtils
{
    public class Timer
    {
        private static double StartTimeStamp = 0;
        private static double EndTimeStamp = 0;
        public static double ElapsedSeconds
        {
            get { return (EndTimeStamp - StartTimeStamp) / 1000; }
        }
        public static double ElapsedMilliseconds
        {
            get { return EndTimeStamp - StartTimeStamp; }
        }
        public static string ElapsedDuration
        {
            get
            {
                int time = Convert.ToInt32(ElapsedSeconds);
                return $"{time / 3600}:{(time - time / 3600 * 3600) / 60:00}:{time % 60:00}";
            }
        }

        public static void Start()
        {
            StartTimeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        public static void End()
        {
            EndTimeStamp = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
