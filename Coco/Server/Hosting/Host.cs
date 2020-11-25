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
            cocoHostBuilder = new CocoHostBuilder();
            return cocoHostBuilder;
        }
    }
}
