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
        public Subject<ParseMessage> ParsedMessage { get; set; }
        public CocoMessageParser()
        {
            ParsedMessage = new Subject<ParseMessage>();
        }
        public void Parser(string text)
        {
            MessageOrigalText = text;
            // 解析

            ParsedMessage.OnNext(new ParseMessage());
        }

        public IObservable<ParseMessage> AfaterParsed()
        {
            return ParsedMessage;
        }
    }
}
