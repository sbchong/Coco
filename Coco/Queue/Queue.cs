using System.Collections.Generic;

namespace Coco.Queue
{
    public class Queue
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
    }
}
