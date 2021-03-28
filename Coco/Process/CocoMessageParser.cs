using Coco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Coco.Process
{
    public class CocoMessageParser
    {
        public string MessageOrigalText { get; set; }
        public Subject<ParseMessage> ParseMessage { get; set; }
        public CocoMessageParser CreateMessageParser(string text)
        {
            MessageOrigalText = text;
            ParseMessage = new Subject<ParseMessage>();
            return this;
        }

        public IObservable<ParseMessage> AfaterParsed()
        {
            return ParseMessage;
        }
    }
}
