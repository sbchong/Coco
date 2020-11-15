using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Coco.Queue
{
    public class Queue
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
    }
}
