using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Server.Hosting
{
    public interface ICocoHost : IDisposable
    {
        IServiceProvider Services { get; set; }

        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));
        void Run();
    }
}