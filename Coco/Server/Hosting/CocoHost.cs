using Coco.Queue;
using Coco.Server.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Coco.Server.Hosting
{
    public class CocoHost : ICocoHost
    {
        public string Host { get ; set ; }
        public string Port { get ; set ; }
        public List<MessageTopic> Topics { get; set; }
        public List<MessageTopic> AckTopics { get ; set ; }
        public IServiceProvider Services { get ; set ; }
        public List<HandleClient> SubcribeClients { get ; set ; }
        public string Persistence { get; set; }

        public List<Broker> Brokers { get; set; }

        public void WriteLogo()
        {
            Console.WriteLine(@"    __ __ __ __              __ __ __ __              __ __ __ __               __ __ __ __    ");
            Console.WriteLine(@"  /    __ __    \          /    __ __    \          /    __ __    \           /    __ __    \  ");
            Console.WriteLine(@" /   /       \   \        /   /       \   \        /   /       \   \         /   /       \   \ ");
            Console.WriteLine(@"|   |         | _ |      |   |         |   |      |   |         | _ |       |   |         |   |");
            Console.WriteLine(@"|   |                    |   |         |   |      |   |                     |   |         |   |");
            Console.WriteLine(@"|   |           _        |   |         |   |      |   |           _         |   |         |   |");
            Console.WriteLine(@"|   |         |   |      |   |         |   |      |   |         |   |       |   |         |   |");
            Console.WriteLine(@" \   \ __ __ /   /        \   \ __ __ /   /        \   \ __ __ /   /         \   \ __ __ /   /");
            Console.WriteLine(@"  \ __ __ __ __ /          \ __ __ __ __ /          \ __ __ __ __ /           \ __ __ __ __ / ");
            Console.WriteLine();
        }

        public void Run()
        {
            Brokers = new List<Broker>();

            var broker = new Broker();

            broker.AddMessage("a");
            broker.AddMessage("b");
            broker.AddMessage("c");

            string result = broker.ToString();


            Topics = new List<MessageTopic>();
            AckTopics = new List<MessageTopic>();
            SubcribeClients = new List<HandleClient>();

            IPAddress iPAddress = IPAddress.Parse(Host ?? "0.0.0.0");
            int port = Convert.ToInt32(Port ?? "9527");
            IPEndPoint ipe = new IPEndPoint(iPAddress, port);

            TcpListener tcpListener = new TcpListener(ipe);

            tcpListener.Start();
            Console.WriteLine("coco start success!!!");
            Console.WriteLine("coco is listening {0}:{1}", ipe.Address.ToString() == "0.0.0.0" ? "[::]" : ipe.Address.ToString(), ipe.Port);
            WriteLogo();

            TcpClient tmpTcpClient;
            while (true)
            {
                try
                {
                    tmpTcpClient = tcpListener.AcceptTcpClient();

                    if (tmpTcpClient.Connected)
                    {
                        var clientId = Guid.NewGuid();
                        HandleClient handleClient = new HandleClient(clientId, tmpTcpClient, this);
                        SubcribeClients.Add(handleClient);

                        handleClient.CommunicateEnd += CommunicateEnd;

                        new Thread(handleClient.Communicate) { IsBackground = true }.Start();
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        public void Push(string topicName, string message)
        {
            var broker = Brokers.FirstOrDefault(x => x.TopicName == topicName);
            if (broker is null)
            {
                if (MsgReceived(topicName, message))
                    return;

                broker = new Broker() { TopicName=topicName};

                Message msg = new Message(message);

                broker.Messages = msg;
            }
            else
            {
                if (MsgReceived(topicName, string.Empty))
                    return;
            }
        }

        public string Pop(string topicName)
        {
            var topic = Topics.FirstOrDefault(x => x.Name == topicName);
            var msg = topic?.Messages.FirstOrDefault();
            if (msg is null) { return null; }
            topic.Messages.Remove(msg);
            return msg;
        }

        public void CommunicateEnd(Guid clientId)
        {
            //Console.WriteLine(HandleClients.Count);
            var client = SubcribeClients.FirstOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                SubcribeClients.Remove(client);
                client = null;
            }
        }

        public bool MsgReceived(string topicName, string msg)
        {
            var client = SubcribeClients.FirstOrDefault(x => x.TopicName == topicName && x.ClientType == 1);
            if (client is null) return false;
            if (client._client is null) { SubcribeClients.Remove(client); return false; }
            msg = Pop(topicName);
            client.SendNewMessage(msg);
            return true;
        }

        //public void AddClient(string topicName, TcpClient tcpClient)
        //{
        //    HoldClients.Add(new Client { TopicName = topicName, TcpClient = tcpClient });
        //}

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
