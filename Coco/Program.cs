using Coco.Hosting.Builder;
using Coco.Hosting.Hosting;

namespace Coco
{
    class Program
    {
        static void Main(string[] args) =>
            CreateCocoHostBuilder(args).Build().Run();

        public static ICocoHostBuilder CreateCocoHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureCocoHostDefaults(cocoBuilder =>
                {
                    //cocoBuilder.UseUrl("0.0.0.0:9522");
                });
    }
}
