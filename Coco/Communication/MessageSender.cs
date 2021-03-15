using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Coco.Enums;
using Coco.Process;

namespace Coco.Communication
{
    public class MessageSender : IDisposable
    {
        private bool disposed;
        private CocoProcesser cocoProcesser;
        private byte[] msgBytes;

        public MessageSender(CocoProcesser processer)
        {
            cocoProcesser = processer;
        }

        public void Send(string message)
        {
            msgBytes = Encoding.UTF8.GetBytes(message);
        }

        public async Task SendAsync(string message, Encoding encoding)
        {
            msgBytes = encoding.GetBytes(message);

           await SendMessageAsync(msgBytes, cocoProcesser.Client);
        }


        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="tmpTcpClient">TcpClient</param>
        public async Task SendMessageAsync(byte[] msgByte, TcpClient tmpTcpClient)
        {
            NetworkStream ns = tmpTcpClient.GetStream();
            if (ns.CanWrite)
            {
                // byte[] msgByte = Encoding.UTF8.GetBytes(msg);
                await ns.WriteAsync(msgByte, 0, msgByte.Length);
            }
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
        ~MessageSender()
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
                msgBytes = null;
            }
            //// 清理非托管资源
            //if (nativeResource != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(nativeResource);
            //    nativeResource = IntPtr.Zero;
            //}
            //让类型知道自己已经被释放
            disposed = true;
            GC.Collect();
        }
    }

}