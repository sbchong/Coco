using Coco.Communication.Base;
using Coco.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Communication
{
    public class MessageReceiver : IDisposable
    {
        private bool disposed;
        private CocoProcesser cocoProcesser;
        private CommunicationBase communication;
        private byte[] msgBytes;
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
        ~MessageReceiver()
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
                communication.Dispose();
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
        }
    }
}
