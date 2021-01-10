using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Coco.Server.Hosting
{
    public class CocoHostBuilder : ICocoHostBuilder
    {
        private ICocoHost cocoHost;

        public string ConfigurationFileName { get => "app.json"; }

        public CocoHostBuilder()
        {
            cocoHost = new CocoHost();
        }

        public CocoHostBuilder(string[] args)
        {
            cocoHost = new CocoHost();

            var filePath = Path.Combine(AppContext.BaseDirectory, ConfigurationFileName);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);

                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    if (document.RootElement.TryGetProperty(Encoding.UTF8.GetBytes("Host"), out var host))
                        cocoHost.Host = host.GetString();
                    if (document.RootElement.TryGetProperty(Encoding.UTF8.GetBytes("Port"), out var port))
                        cocoHost.Port = port.GetString();
                }
            }




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