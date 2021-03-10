using Coco.Hosting.Hosting;
using System;
using System.Threading.Tasks;

namespace Coco.Hosting.Builder
{
    public interface ICocoHostBuilder
    {
        ICocoHost Build();
        ICocoHostBuilder ConfigureCocoHostDefaults(string[] args);
        void UseUrl(string url);
    }
}
