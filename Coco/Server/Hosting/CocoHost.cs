using Coco.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Server.Hosting
{
    public class CocoHost : ICocoHost
    {
        public static List<MessageTopic> Topics { get; set; } = new List<MessageTopic>();

        public static List<MessageTopic> AckTopics { get; set; } = new List<MessageTopic>();
        public IServiceProvider Services { get; set; }

        public void Run()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 9527);

            TcpListener tcpListener = new TcpListener(ipe);

            tcpListener.Start();
            Console.WriteLine("coco start success!!!");
            Console.WriteLine("coco is listening {0}:{1}", ipe.Address.ToString() == "0.0.0.0" ? "[::]" : ipe.Address.ToString(), ipe.Port);

            TcpClient tmpTcpClient;
            while (true)
            {
                try
                {
                    tmpTcpClient = tcpListener.AcceptTcpClient();

                    if (tmpTcpClient.Connected)
                    {
                        HandleClient handleClient = new HandleClient(tmpTcpClient, this);
                        new Thread(handleClient.Communicate) { IsBackground = true }.Start();                  
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            } // end while
        } // end ListenToConnect()

        public void Push(string channelName, string msg)
        {
            var topic = Topics.FirstOrDefault(x => x.Name == channelName);
            if (topic is null)
            {
                topic = new MessageTopic
                {
                    Name = channelName,
                    Messages = new List<Message> { new Message { Body = msg } }
                };

                Topics.Add(topic);
            }
            else
            {
                topic.Messages.Add(new Message { Body = msg });
            }
        }

        public string Pop(string channelName)
        {
            var topic = Topics.FirstOrDefault(x => x.Name == channelName);
            var msg = topic?.Messages.FirstOrDefault();
            if (msg is null) { return null; }
            topic.Messages.Remove(msg);
            return msg.Body;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
