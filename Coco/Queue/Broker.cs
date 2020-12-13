using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Queue
{
    public class Broker
    {
        public string Topic { get; set; }
        public Message Messages { get; set; }
    }
}
