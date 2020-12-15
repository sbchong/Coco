namespace Coco.Queue
{
    public class Message
    {
        public Message(string data)
        {
            Data = data;
        }

        public string Data { get; set; }
        public Message Previous { get; set; }
        public Message Next { get; set; }
    }
}