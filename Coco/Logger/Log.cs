using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Logger
{
    /// <summary>
    /// 日志记录，分为两个等级，information和error，使用滚动文件记录，在应用启动时检查日志文件
    /// </summary>
    public static class Log
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".log");

        private static string logFileName = string.Empty;

        private static bool init = false;

        private static bool ok = true;

        private static List<string> logs = new List<string>();

        public static void Init()
        {
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
                File.Create(logFileName);
            }
            init = true;
        }

        public static void LogInformation(string infomation)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("INFO ==> ");
            Console.ForegroundColor = ConsoleColor.White;
            var value = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {infomation}\n";
            Console.WriteLine("\t" + value);
            LogAsync(value);
        }

        public static void LogError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR ==> ");
            Console.ForegroundColor = ConsoleColor.White;
            var value = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {error}";
            Console.WriteLine("\t" + value);
            LogAsync(value);
        }

        private static async Task LogAsync(string value)
        {
            if (!init)
            {
                Init();
            }
            logs.Add(value);
            if (ok)
            {
                await WriteToFileAsync();
            }
        }

        private static async Task WriteToFileAsync()
        {
            //foreach (var log in logs)
            //{
            //File.AppendAllLines(logFileName, logs);
            //}
            logFileName = Path.Combine(logFilePath, $"{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
            if (File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }
            using var fs = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            foreach (var log in logs)
            {
                await fs.WriteAsync(Encoding.UTF8.GetBytes(log));
            }
            ok = true;
        }
    }
}