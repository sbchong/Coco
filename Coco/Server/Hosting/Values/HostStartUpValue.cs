using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Server.Hosting.Values
{
    public static class HostStartUpValue
    {
        public static string Host { get; set; } = "127.0.0.1";
        public static string Port { get; set; } = "9527";
        public static bool IsPersistence { get; set; } = false;
        public static string PersistenceType { get; set; } = "";
    }
}
