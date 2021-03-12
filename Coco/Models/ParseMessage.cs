using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Models
{
    public class ParseMessage
    {
        public ClientType Type { get; set; }
        public string TopicName { get; set; }
        public string Message { get; set; }
    }
}
