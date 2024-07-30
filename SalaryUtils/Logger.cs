using System.Diagnostics;

namespace SalaryUtils
{
    public static class Logger
    {
        static readonly string logDirectory = "Logs";
        static readonly string logFileName = $"{Process.GetCurrentProcess().ProcessName}.log";
        static readonly object lockObject = new();

        static Logger()
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public static void Info(object message)
        {
            lock (lockObject)
            {
                try
                {
                    CheckLogFile();
                    var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";
                    Console.Write(logEntry);
                    File.AppendAllText(Path.Combine(logDirectory, logFileName), logEntry);
                }
                catch { }
            }
        }

        private static void CheckLogFile()
        {
            var filePath = Path.Combine(logDirectory, logFileName);
            if (!File.Exists(filePath))
                return;
            if (DateTime.Now.Date > File.GetLastWriteTime(filePath).Date)
            {
                // Create a new log file for the current date
                var newFileName = $"{Path.GetFileNameWithoutExtension(logFileName)}_{DateTime.Now:yyyyMMdd}{Path.GetExtension(logFileName)}";
                File.Move(filePath, Path.Combine(logDirectory, newFileName));
            }
        }
    }
}
