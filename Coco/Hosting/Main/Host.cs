using Coco.Hosting.Builder;

namespace Coco.Hosting.Main
{
    public static class Host
    {
        private static ICocoHostBuilder cocoHostBuilder = null;

        public static ICocoHostBuilder CreateDefaultBuilder(string[] args)
        {
            if (cocoHostBuilder == null)
            {
                cocoHostBuilder = new CocoHostBuilder().ConfigureCocoHostDefaults(args);
            }
            return cocoHostBuilder;
        }
    }
}
