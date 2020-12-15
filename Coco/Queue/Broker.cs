using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Queue
{
    public class Broker
    {
        public string TopicName { get; set; }

        public Message Messages { get; set; }

        private Message Head { get; set; }
        private Message End { get; set; }

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
