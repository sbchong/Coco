using System;
using System.Net.Sockets;
using System.Text;

namespace Coco.Communication.Base
{
    public class CommunicationBase : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="tmpTcpClient">TcpClient</param>
        public void SendMsg(string msg, TcpClient tmpTcpClient)
        {
            NetworkStream ns = tmpTcpClient.GetStream();
            if (ns.CanWrite)
            {
                byte[] msgByte = Encoding.UTF8.GetBytes(msg);
                ns.Write(msgByte, 0, msgByte.Length);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="tmpTcpClient">TcpClient</param>
        /// <returns>消息</returns>
        public string ReceiveMsg(TcpClient tmpTcpClient)
        {
            string receiveMsg = string.Empty;
            byte[] receiveBytes = new byte[tmpTcpClient.ReceiveBufferSize];
            int numberOfBytesRead = 0;
            NetworkStream ns = tmpTcpClient.GetStream();

            if (ns.CanRead)
            {
                do
                {
                    numberOfBytesRead = ns.Read(receiveBytes, 0, tmpTcpClient.ReceiveBufferSize);
                    receiveMsg += Encoding.UTF8.GetString(receiveBytes, 0, numberOfBytesRead);
                }
                while (ns.DataAvailable);
            }
            return receiveMsg;
        }

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
        ~CommunicationBase()
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
                //if (_client != null)
                //{
                //    _client.Dispose();
                //    _client = null;
                //}
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