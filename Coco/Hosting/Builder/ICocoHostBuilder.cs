using Coco.Hosting.Hosting;
using System;

namespace Coco.Hosting.Builder
{
    public interface ICocoHostBuilder
    {
        ICocoHost Build();
        ICocoHostBuilder ConfigureCocoHostDefaults(Action<ICocoHostBuilder> configureDelegate);
        void UseUrl(string url);
    }
}
