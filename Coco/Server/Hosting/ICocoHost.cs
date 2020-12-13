using Coco.Queue;
using Coco.Server.Communication;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Server.Hosting
{
    public interface ICocoHost : IDisposable
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public List<MessageTopic> Topics { get; set; }
        public List<MessageTopic> AckTopics { get; set; } 
        public IServiceProvider Services { get; set; }
        public List<HandleClient> SubcribeClients { get; set; }
        public string Persistence { get; set; }
        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
        void Run();
    }
}