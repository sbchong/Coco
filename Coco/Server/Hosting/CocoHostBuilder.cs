using Coco.Server.Hosting.Values;
using System;

namespace Coco.Server.Hosting
{
    public class CocoHostBuilder : ICocoHostBuilder
    {
        private ICocoHost cocoHost;

        public int Id { get; set; } = 0;

        public CocoHostBuilder()
        {
            cocoHost = new CocoHost();
        }

        public CocoHostBuilder(string[] args)
        {
            cocoHost = new CocoHost();
            if (args.Length <= 1 || args == null)
                return;
            else
            {
                for (int i = 0; i < args.Length; i += 2)
                {
                    switch (args[i].Remove(0, 2).ToUpper())
                    {
                        case "HOST": cocoHost.Host = args[i + 1]; break;
                        case "PORT": cocoHost.Port = args[i + 1]; break;
                        case "PERSISTENCE": cocoHost.Persistence = args[i + 1]; break;
                    }
                }
            }
        }


        public ICocoHostBuilder ConfigureCocoHostDefaults(Action<ICocoHostBuilder> configureDelegate)
        {
            configureDelegate(this);
            return this;
        }

        public void UseUrl(string url)
        {
            string[] items = url.Split(':');
            cocoHost.Host = items[0];
            cocoHost.Port = items[1];
        }

        public void UsePersistence(string persistence)
        {
            cocoHost.Persistence = persistence;
        }


        public ICocoHost Build()
        {
            return cocoHost;
        }
    }
}