using System.Reactive.Subjects;

namespace Coco.Queue
{
    public class Broker
    {
        public string TopicName { get; }

        public Message Messages { get; set; }

        /// <summary>
        /// 添加Rx支持，将使用Rx作为中心
        /// </summary>
        public Subject<Message> AfterAddMessage { get; set; } = new Subject<Message>();

        private Message Head { get; set; }
        private Message End { get; set; }

        public Broker(string topicName)
        {
            TopicName = topicName;
        }

        /// <summary>
        /// 收到消息，存储消息并使用Rx分发
        /// </summary>
        /// <param name="message"></param>
        public void AddMessage(string message)
        {
            Message msg = new Message(message);
            if (Head == null)
            {
                Messages = msg;
                Head = Messages;
                End = Messages;
                return;
            }

            End.Next = msg;
            msg.Previous = End;
            End = msg;

            AfterAddMessage.OnNext(msg);
        }

        public string GetMessage()
        {
            if (Head == null || Messages == null)
            {
                return null;
            }
            string data = Messages.Data;
            Messages = Messages.Next;
            Head = Messages;
            return data;
        }

        public override string ToString()
        {
            var temp = Messages;
            string result = string.Empty;
            while (true)
            {
                result += temp.Data;

                if (temp.Next == null)
                    break;
                temp = temp.Next;
            }
            return result;
        }
    }
}
