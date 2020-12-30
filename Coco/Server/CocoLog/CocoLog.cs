using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Server.CocoLog
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
                init = true;
            }
            //else
            //{
            //    if (File.Exists(logFileName))
            //    {
            //        File.Create(logFileName);
            //        init = true;
            //    }
            //    else
            //    {
            //        init = true;
            //    }
            //}
            init = true;
        }

        public static void LogInformation(string infomation)
        {
            Console.WriteLine(infomation);
            if (!init)
            {
                Init();
            }
            logs.Add($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] {infomation}\n");
            if (ok)
            {
                new Thread(WriteToFile) { IsBackground = true }.Start();
            }
        }

        public static void WriteToFile()
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