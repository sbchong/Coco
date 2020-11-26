using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Coco.Server
{
    public class Client
    {
        public string TopicName { get; set; }
        public TcpClient TcpClient { get; set; }
    }
}
