using Coco.Communication;
using Coco.Hosting.Hosting;
using Coco.Logger;
using Coco.Models;
using System;
using System.Net.Sockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Coco.Process
{
    public class CocoProcesser : IDisposable
    {
        private bool disposed = false;
        public bool Flag { get; set; } = false;

        public TcpClient Client { get; set; }
        public CocoHost Server { get; set; }
        public string TopicName { get; set; }
        public MessageSender Sender { get; set; }
        public MessageReceiver Receiver { get; set; }
        public Guid Id { get; set; }
        public ClientType ClientType { get; set; }

        public event Action<CocoProcesser> CommunicationStart;
        public event Action<Guid> CommunicateEnd;

        public Subject<ParseMessage> AfterParseMessage { get; set; } = new Subject<ParseMessage>();

        public Subject<Guid> AfterClosed { get; set; } = new Subject<Guid>();

        public CocoProcesser(TcpClient client, CocoHost server)
        {
            Client = client;
            Server = server;

            Sender = new MessageSender(this);
            Receiver = new MessageReceiver(this);

            // CommunicationStart += Server.AddSubscribeClient;
            //CommunicateEnd += Server.RemoveSubscribeClient;

        }

        public async Task Run()
        {
            MessageReceiver receiver = new(this);
            string temp = await receiver.ReceiveMessageAsync();
            CocoMessageParser cocoMessageParser = new();
            //cocoMessageParser.Parse();

            cocoMessageParser.AfaterParsed().Subscribe(result =>
            {
                Server.Push(result.TopicName, result.Message);
            });
        }


        public void Communicate()
        {
            try
            {
                string content = Receiver.ReceiveMessageAsync(Client);

                var part = content.Split("^^^");

                var method = part[0];
                TopicName = part[1];

                ClientType = Enum.Parse<ClientType>(method);

                //生产者发布消息后立即断开
                if (ClientType == ClientType.Producer)
                {
                    string msg = part[2];
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Server.Push(TopicName, msg);
                    }
                    SendCompeleted();

                    CommunicateEnd?.Invoke(this.Id);
                }

                //只有消费者会被维护链接
                else if (ClientType == ClientType.Comsummer)
                {
                    CommunicationStart.Invoke(this);
                    if (TrySendMessage(TopicName))
                    {
                        CommunicateEnd?.Invoke(this.Id);
                    }
                    else
                    {
                        var time = DateTime.Now.AddSeconds(15);
                        Thread.Sleep(15000);
                        SendCompeleted();
                        CommunicateEnd?.Invoke(this.Id);
                    }
                }
                else this.Client.Close();
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                this.Client.Close();
                CommunicateEnd?.Invoke(this.Id);
            }
        }


        public void SendCompeleted()
        {
            //Communication.SendMsg($"Ok", this.Client);
        }

        public bool TrySendMessage(string topicName)
        {
            var msg = Server.Pop(topicName);
            if (!string.IsNullOrEmpty(msg))
            {
                //Communication.SendMsg(msg, this.Client);
                return true;
            }
            else
                return false;
        }

        public void SendNewMessage(string msg)
        {
            if (Client is null)
            {
                CommunicateEnd?.Invoke(this.Id);
                return;
            }
            //Communication.SendMsg(msg, Client);
            //Communication.Close();
            CommunicateEnd?.Invoke(this.Id);
        }

        private void Send(object sender, string msg) { }//Communication.SendMsg(msg, this.Client);

        public void Dispose()
        {
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
        public void Close()
        {
            Dispose();
        }

        ///<summary>
        /// 必须，以备忘记了显式调用Dispose方法
        ///</summary>
        ~CocoProcesser()
        {
            //必须为false
            Dispose(false);
        }

        ///<summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        ///</summary>
        ///<param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                if (Client != null)
                {
                    Client.Close();
                    Client.Dispose();
                    Client = null;
                }
            }
            //// 清理非托管资源
            //if (nativeResource != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(nativeResource);
            //    nativeResource = IntPtr.Zero;
            //}
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}
