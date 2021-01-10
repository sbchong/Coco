using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Coco.Server
{
    public static class CocoLog
    {
        private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".log");

        private static readonly string logFileName = Path.Combine(logFilePath, "running.log");

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
            var value = $"INFO ==> \n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {infomation}\n";
            Console.WriteLine(value);
            Log(value);
        }

        public static void LogError(string error)
        {
            var value = $"ERROR ==> \n[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {error}";
               Console.WriteLine(value);
            Log(value);
        }

        private static void Log(string value)
        {
            if (!init)
            {
                Init();
            }
            logs.Add(value);
            if (ok)
            {
                new Thread(WriteToFile) { IsBackground = true }.Start();
            }
        }

        private static void WriteToFile()
        {
            //foreach (var log in logs)
            //{
            //File.AppendAllLines(logFileName, logs);
            //}
            if (File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }
            using var fs = new FileStream(logFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            foreach (var log in logs)
            {
                fs.Write(Encoding.UTF8.GetBytes(log));
            }
            ok = true;
        }
    }
}