
using System;
namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal class ErrorProcessor : ElementProcessorBase
    {
        private Exception exception;

        public override void Process(string targetDirectory)
        {
            var inititialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Failed to process: " + this.exception.Message);
            Console.ForegroundColor = inititialColor;
        }

        public ErrorProcessor(Url url, Action<Url> actionOnNewUrl, Exception exception)
            : base(url, actionOnNewUrl)
        {
            this.exception = exception;
            ErrorProcessor.Errors++;
        }

        public static int Errors { get; private set; }
    }
}
