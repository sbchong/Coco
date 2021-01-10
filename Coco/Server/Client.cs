using System.Net.Sockets;

namespace Coco.Server
{
    public class Client
    {
        public string TopicName { get; set; }
        public TcpClient TcpClient { get; set; }
    }
}
