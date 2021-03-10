using Coco.Hosting.Builder;

namespace Coco.Hosting.Hosting
{
    public static class Host
    {
        private static ICocoHostBuilder cocoHostBuilder = null;

        public static ICocoHostBuilder CreateDefaultBuilder()
        {
            if (cocoHostBuilder == null) cocoHostBuilder = new CocoHostBuilder();
            return cocoHostBuilder;
        }

        public static ICocoHostBuilder CreateDefaultBuilder(string[] args)
        {
            if (cocoHostBuilder == null)
                cocoHostBuilder = new CocoHostBuilder(args);
            return cocoHostBuilder;
        }
    }
}
