using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Coco.Queue
{
    public class MessageTopic
    {
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
    }
}
