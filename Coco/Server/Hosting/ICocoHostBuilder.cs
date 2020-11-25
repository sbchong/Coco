using System;
using System.Collections.Generic;
using System.Text;

namespace Coco.Server.Hosting
{
    public interface ICocoHostBuilder
    {
        int Id { get; set; }

        ICocoHost Build();
        ICocoHostBuilder ConfigureCocoHostDefaults(Action<ICocoHostBuilder> configureDelegate);
    }
}
