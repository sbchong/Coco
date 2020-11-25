using Coco.Server.Hosting;
using System;
using System.Net.Sockets;

namespace Coco.Server
{
    public class HandleClient : IDisposable
    {
        private TcpClient _client;
        private CocoHost _server;

        private string channelName;
        private volatile string Msg = null;

        private bool flag = true;

        private bool disposed = false;

        CommunicationBase cb = new CommunicationBase();

        event EventHandler<string> MsgUpdated;

        public HandleClient(TcpClient client, CocoHost server)
        {
            _client = client;
            _server = server;



            //Thread th = new Thread(new ThreadStart(UpadateMag));
            //th.IsBackground = true;
            // th.Start();
        }

        public void UpadateMag()
        {
            while (true)
            {
                Msg = _server.Pop(channelName);
                if (!string.IsNullOrEmpty(Msg))
                    MsgUpdated?.Invoke(this, Msg);
            }
        }

        public void Communicate()
        {
            try
            {
                string content = cb.ReceiveMsg(this._client);

                var part = content.Split('\\');

                var method = part[0];
                var channelName = part[1];

                switch (Convert.ToInt32(method))
                {
                    case 0:
                        string msg = part[2];
                        if (!string.IsNullOrEmpty(msg))
                        {
                            _server.Push(channelName, msg);
                        }
                        break;
                    case 1:
                        SendMessage(channelName);
                        break;
                    default: break;
                }
            }
            catch
            {
                this._client.Close();
            }
            finally
            {
                this._client.Close();
            }
        }

        public void SendCompeleted()
        {
            cb.SendMsg($"Ok", this._client);
        }
        public void SendMessage(string channelName)
        {
            //var expire = DateTime.Now.AddSeconds(10);
            //while (DateTime.Now < expire)
            //{
            //    Msg = _server.Pop(channelName);
            //    if (!string.IsNullOrEmpty(Msg))
            //        cb.SendMsg(Msg, this._client);
            //}
            Msg = _server.Pop(channelName);
            cb.SendMsg(Msg, this._client);
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
        /// 必须，以备程序员忘记了显式调用Dispose方法
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
