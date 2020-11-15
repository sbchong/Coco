using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Coco.Server
{
    public class HandleClient
    {
        private TcpClient _client;
        private CocoServer _server;

        private string channelName;
        private volatile string Msg = null;

        private bool flag = true;

        CommunicationBase cb = new CommunicationBase();

        event EventHandler<string> MsgUpdated;

        public HandleClient(TcpClient client, CocoServer server)
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
                string method = cb.ReceiveMsg(this._client);
                //Console.WriteLine(method + "\n");
                SendCompeleted();
                if (method == 0.ToString())
                {
                    channelName = cb.ReceiveMsg(this._client);
                    //Console.WriteLine(channelName);
                    SendCompeleted();
                    string msg = cb.ReceiveMsg(this._client);
                    //Console.WriteLine(msg + "\n");
                    if (!string.IsNullOrEmpty(msg))
                    {
                        _server.Push(channelName, msg);
                    }

                    //Thread.Sleep(15000);
                    SendCompeleted();
                }
                else
                {
                    string channelName = cb.ReceiveMsg(this._client);
                    //Console.WriteLine(channelName);
                    SendMessage(channelName);
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

        private void Send(object sender,string msg) => cb.SendMsg(msg, this._client);
    }
}
