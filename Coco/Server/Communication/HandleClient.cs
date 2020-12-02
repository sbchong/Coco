using Coco.Server.Hosting;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Coco.Server.Communication
{
    public class HandleClient : IDisposable
    {
        public TcpClient _client;
        public CocoHost _server;

        private string topicName;
        private volatile string Msg = null;

        public bool Flag { get; set; } = false;

        private bool disposed = false;

        CommunicationBase cb;

        public event Action<Guid> CommunicateEnd;

        public Guid Id { get; set; }
        public int ClientType { get; set; }
        public string TopicName { get => topicName; set => topicName = value; }

        public HandleClient(Guid id,TcpClient client, CocoHost server)
        {
            _client = client;
            _server = server;
            Id = id;
            cb = new CommunicationBase();
        }

        public void Communicate()
        {
            try
            {
                string content = cb.ReceiveMsg(this._client);

                var part = content.Split("^^^");

                var method = part[0];
                topicName = part[1];

                ClientType = Convert.ToInt32(method);

                if (ClientType == 0)
                {
                    string msg = part[2];
                    if (!string.IsNullOrEmpty(msg))
                    {
                        _server.Push(topicName, msg);
                    }
                    SendCompeleted();
                    this._client.Close();
                    CommunicateEnd?.Invoke(this.Id);
                }
                else if (ClientType == 1)
                {
                    if (SendMessage(topicName))
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
                else this._client.Close();
            }
            catch (Exception ex)
            {
                this._client.Close();
                CommunicateEnd?.Invoke(this.Id);
            }
        }

        public void SendCompeleted()
        {
            cb.SendMsg($"Ok", this._client);
        }

        public bool SendMessage(string topicName)
        {
            Msg = _server.Pop(topicName);
            if (!string.IsNullOrEmpty(Msg))
            {
                cb.SendMsg(Msg, this._client);
                return true;
            }
            else
                return false;
        }

        public void SendNewMessage(string msg)
        {
            if (_client is null)
            {
                CommunicateEnd?.Invoke(this.Id);
                return;
            }
            cb.SendMsg(msg, this._client);
            cb.Close();
            CommunicateEnd?.Invoke(this.Id);
        }

        private void Send(object sender, string msg) => cb.SendMsg(msg, this._client);

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
        ~HandleClient()
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
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
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
