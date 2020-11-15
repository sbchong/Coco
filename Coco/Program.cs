using Coco.Server;
using System;

namespace Coco
{
    class Program
    {
        static void Main(string[] args)
        {
            CocoServer coco = new CocoServer();
            coco.Start();
        }
    }
}
