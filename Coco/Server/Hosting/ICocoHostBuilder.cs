using System;

namespace Coco.Server.Hosting
{
    public interface ICocoHostBuilder
    {
        ICocoHost Build();
        ICocoHostBuilder ConfigureCocoHostDefaults(Action<ICocoHostBuilder> configureDelegate);
        void UseUrl(string url);
    }
}
