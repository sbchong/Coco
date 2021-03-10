using Coco.Hosting.Builder;
using Coco.Hosting.Hosting;
using Coco.Hosting.Main;

namespace Coco
{
    class Program
    {
        static void Main(string[] args) =>
            CreateCocoHostBuilder(args).Build().Run();

        public static ICocoHostBuilder CreateCocoHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);
    }
}
