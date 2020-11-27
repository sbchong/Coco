using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Server.Hosting
{
    public static class Host
    {
        private static ICocoHostBuilder cocoHostBuilder = null;

        public static ICocoHostBuilder CreateDefaultBuilder()
        {
            throw null;
        }

        public static ICocoHostBuilder CreateDefaultBuilder(string[] args)
        {
            Console.WriteLine(string.Join(",", args));
            cocoHostBuilder = new CocoHostBuilder();
            return cocoHostBuilder;
        }
    }
}
