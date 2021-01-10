namespace Coco.Queue
{
    public class Broker
    {
        public string TopicName { get; }

        public Message Messages { get; set; }

        private Message Head { get; set; }
        private Message End { get; set; }

        public Broker(string topicName)
        {
            TopicName = topicName;
        }

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
