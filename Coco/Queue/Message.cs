namespace Coco.Queue
{
    public class Message
    {
        public Message(string body)
        {
            Body = body;
        }
        public string Body { get; }
        public Message Next { get; }
    }
}