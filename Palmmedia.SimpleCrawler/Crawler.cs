using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Palmmedia.SimpleCrawler.ElementProcessing;

namespace Palmmedia.SimpleCrawler
{
    public class Crawler
    {
        private readonly string targetDirectory;

        private readonly HashSet<Url> processedElements = new HashSet<Url>();

        private readonly HashSet<Url> elementsToProcess = new HashSet<Url>();

        public Crawler(string baseUri, string targetDirectory)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            if (targetDirectory == null)
            {
                throw new ArgumentNullException(nameof(targetDirectory));
            }

            string host = baseUri.Substring(0, baseUri.LastIndexOf('/') + 1);
            this.targetDirectory = targetDirectory;

            this.OnNewUrlFound(new Url(host, baseUri));
        }

        public void Start()
        {
            if (!Directory.Exists(this.targetDirectory))
            {
                Directory.CreateDirectory(this.targetDirectory);
            }

            while (this.elementsToProcess.Count > 0)
            {
                Url urlToProcess = this.elementsToProcess.First();
                this.elementsToProcess.Remove(urlToProcess);
                this.processedElements.Add(urlToProcess);

                var processor = ElementProcessing.ElementProcessorFactory.CreateElementProcessor(
                    urlToProcess, 
                    this.OnNewUrlFound);

                processor.Process(this.targetDirectory);
            }

            Console.Write("Crawled " + this.processedElements.Count + " elements.");
            var inititialColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Errors: " + ErrorProcessor.Errors);
            Console.ForegroundColor = inititialColor;
        }

        private void OnNewUrlFound(Url url)
        {
            if (url.EligibleForCrawling
                && !processedElements.Contains(url) 
                && !elementsToProcess.Contains(url))
            {
                this.elementsToProcess.Add(url);
            }
        }
    }
}
