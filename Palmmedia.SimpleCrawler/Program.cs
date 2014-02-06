using System;

namespace Palmmedia.SimpleCrawler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 2)
            {
                Console.WriteLine("Please pass the base URL and the target directory");
                Console.ReadKey();
                return;
            }

            Crawler crawler = new Crawler(args[0], args[1]);
            crawler.Start();

            Console.ReadKey();
        }
    }
}
