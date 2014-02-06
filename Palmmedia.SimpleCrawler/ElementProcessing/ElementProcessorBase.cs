using System;
using System.IO;
using System.Text;

namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal abstract class ElementProcessorBase
    {
        protected ElementProcessorBase(Url url, Action<Url> actionOnNewUrl)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            if (actionOnNewUrl == null)
            {
                throw new ArgumentNullException("actionOnNewUrl");
            }

            this.Url = url;
            this.ActionOnNewUrl = actionOnNewUrl;
        }

        protected ElementProcessorBase(Url url, Action<Url> actionOnNewUrl, byte[] data)
            : this(url, actionOnNewUrl)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this.Data = data;
        }

        protected byte[] Data { get; private set; }

        protected string Text 
        {
            get
            {
                return Encoding.UTF8.GetString(this.Data);
            }
        }

        protected Url Url { get; private set; }

        protected Action<Url> ActionOnNewUrl { get; private set; }

        public abstract void Process(string targetDirectory);

        protected void Save(string targetDirectory, byte[] processedData)
        {
            string path = Path.Combine(targetDirectory, this.Url.PathRelativeToHost);
            string directory = new FileInfo(path).Directory.FullName;

            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (IOException)
                {
                    var inititialColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Failed to create directory: " + directory);
                    Console.ForegroundColor = inititialColor;
                }
            }

            try
            {
                File.WriteAllBytes(path, processedData);
            }
            catch (IOException)
            {
                var inititialColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Failed to create file: " + path);
                Console.ForegroundColor = inititialColor;
            }
        }
    }
}
