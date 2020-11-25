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
        public ICocoHost Build()
        {
            return cocoHost;
        }

        public ICocoHostBuilder ConfigureCocoHostDefaults(Action<ICocoHostBuilder> configureDelegate)
        {

            configureDelegate(this);

            return this;
        }
    }
}