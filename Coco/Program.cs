using System;
using Coco.Server;

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
