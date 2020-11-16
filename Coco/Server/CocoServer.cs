using Coco.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;


namespace Coco.Server
{
    public class CocoServer
    {
        private List<HandleClient> clients;
        public static List<MessageTopic> Topics { get; set; } = new List<MessageTopic>();

        public static List<MessageTopic> AckTopics { get; set; } = new List<MessageTopic>();

        public void Start()
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 9527);

            TcpListener tcpListener = new TcpListener(ipe);

            tcpListener.Start();
            Console.WriteLine("coco start success!!! \n");

            TcpClient tmpTcpClient;
            while (true)
            {
                try
                {
                    tmpTcpClient = tcpListener.AcceptTcpClient();

                    if (tmpTcpClient.Connected)
                    {
                        HandleClient handleClient = new HandleClient(tmpTcpClient, this);
                        Thread myThread = new Thread(new ThreadStart(handleClient.Communicate))
                        { IsBackground = true, Name = tmpTcpClient.Client.RemoteEndPoint.ToString() };
                        myThread.Start();
                    }
                }
                catch (Exception ex)
                {
                    tmpTcpClient.Close();
                }
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
            if (topic is null)
                return null;
            var msg = topic.Messages.FirstOrDefault();
            if (msg is null) { return null; }
            topic.Messages.Remove(msg);
            return msg.Body;
        }
    }
}
