using Coco.Hosting.Extension;
using Coco.Logger;
using Coco.Process;
using Coco.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Hosting.Hosting
{
    public class CocoHost : ICocoHost
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public List<MessageTopic> Topics { get; set; }
        public List<MessageTopic> AckTopics { get; set; }
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// 消费者客户端
        /// </summary>
        public List<CocoProcesser> SubcribeClients { get; set; }


        public string Persistence { get; set; }

        /// <summary>
        /// 消息管道
        /// </summary>
        public List<Broker> Brokers { get; set; }

        public TcpListener CocoListenner { get; set; }

        /// <summary>
        /// 启动COCO主机，初始化数据，绑定网络
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            Init();
            this.WriteStartInfo();

            try
            {
                IPAddress iPAddress = IPAddress.Parse(Host ?? "0.0.0.0");
                int port = Convert.ToInt32(Port ?? "9527");
                var ipe = new IPEndPoint(iPAddress, port);
                CocoListenner = new TcpListener(ipe);

                CocoListenner.Start();

                Log.LogInformation("coco start success!!!");
                Log.LogInformation($"coco is listening {Host}:{Port}");



                Accept();
            }
            catch (Exception ex)
            {
                Log.LogError($"coco start error : {ex.Message}");
            }
        }

        public void Init()
        {
            Brokers = new List<Broker>();
            Topics = new List<MessageTopic>();
            AckTopics = new List<MessageTopic>();
            SubcribeClients = new List<CocoProcesser>();
        }

        private void Accept()
        {
            while (true)
            {
                try
                {

                    TcpClient tmpTcpClient = CocoListenner.AcceptTcpClient();

                    if (tmpTcpClient.Connected)
                    {
                        Task.Run(() =>
                        {
                            Guid clientId = Guid.NewGuid();
                            CocoProcesser processer = new(tmpTcpClient, this);



                            //TODO: 使用RX
                            processer.AfterParseMessage.Subscribe();

                            processer.AfterClosed.Subscribe(x =>
                            {
                                Log.LogInformation(x.ToString());
                            });

                        });
                    }
                }
                catch (Exception ex)
                {
                    Log.LogError(ex.Message);
                    break;
                }
            }
        }

        public void AddSubscribeClient(CocoProcesser client)
        {
            SubcribeClients.Add(client);
        }

        public void RemoveSubscribeClient(Guid clientId)
        {
            //Console.WriteLine(HandleClients.Count);
            var client = SubcribeClients.FirstOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                SubcribeClients.Remove(client);
                client = null;
            }
        }


        public void Push(string topicName, string message)
        {
            var broker = Brokers.FirstOrDefault(x => x.TopicName == topicName);
            if (broker is null)
            {
                broker = new Broker(topicName);
                Brokers.Add(broker);
            }

            broker.AddMessage(message);
            new Thread(() => MsgReceived(topicName, message)) { IsBackground = true }.Start();
        }

        public string Pop(string topicName)
        {
            var broker = Brokers.FirstOrDefault(x => x.TopicName == topicName);
            if (broker == null)
            {
                return null;
            }
            var msg = broker.GetMessage();
            return msg;
        }

        //[Obsolete]
        //public string Pop1(string topicName)
        //{
        //    var topic = Topics.FirstOrDefault(x => x.Name == topicName);
        //    var msg = topic?.Messages.FirstOrDefault();
        //    if (msg is null) { return null; }
        //    topic.Messages.Remove(msg);
        //    return msg;
        //}



        public void MsgReceived(string topicName, string msg)
        {
            var client = SubcribeClients.FirstOrDefault(x => x.TopicName == topicName);
            if (client is null) return;
            if (client.Client is null)
            {
                SubcribeClients.Remove(client);
                return;
            }
            msg = Pop(topicName);
            if (!string.IsNullOrEmpty(msg))
            {
                client.SendNewMessage(msg);
            }
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
