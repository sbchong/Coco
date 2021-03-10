using Coco.Hosting.Hosting;
using System;

namespace Coco.Hosting.Extension
{
    public static class HostStartExtension
    {
        public static ICocoHost WriteStartInfo(this ICocoHost coco)
        {
            //Console.WriteLine("coco start success!!!");
            //Console.WriteLine("coco is listening {0}:{1}", Host, Port);
            Console.WriteLine(@"    __ __ __ __              __ __ __ __              __ __ __ __               __ __ __ __    ");
            Console.WriteLine(@"  /    __ __    \          /    __ __    \          /    __ __    \           /    __ __    \  ");
            Console.WriteLine(@" /   /       \   \        /   /       \   \        /   /       \   \         /   /       \   \ ");
            Console.WriteLine(@"|   |         | _ |      |   |         |   |      |   |         | _ |       |   |         |   |");
            Console.WriteLine(@"|   |                    |   |         |   |      |   |                     |   |         |   |");
            Console.WriteLine(@"|   |           _        |   |         |   |      |   |           _         |   |         |   |");
            Console.WriteLine(@"|   |         |   |      |   |         |   |      |   |         |   |       |   |         |   |");
            Console.WriteLine(@" \   \ __ __ /   /        \   \ __ __ /   /        \   \ __ __ /   /         \   \ __ __ /   /");
            Console.WriteLine(@"  \ __ __ __ __ /          \ __ __ __ __ /          \ __ __ __ __ /           \ __ __ __ __ / ");
            Console.WriteLine();
            return coco;
        }
    }
}
