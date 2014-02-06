using System;
using System.IO;
using System.Net;

namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal static class ElementProcessorFactory
    {
        public static ElementProcessorBase CreateElementProcessor(Url url, Action<Url> actionOnNewUrl)
        {
            Console.WriteLine("Processing: " + url);

            try
            {
                WebRequest request = WebRequest.Create(url.Uri);
                using (WebResponse response = request.GetResponse())
                {
                    byte[] data = ReadBinaryResponse(response);

                    if (response.ContentType.StartsWith("text/html"))
                    {
                        return new HtmlProcessor(url, actionOnNewUrl, data);
                    }
                    else if (response.ContentType.StartsWith("text/css"))
                    {
                        return new CssProcessor(url, actionOnNewUrl, data);
                    }
                    else if (response.ContentType.StartsWith("text/javascript"))
                    {
                        return new JsProcessor(url, actionOnNewUrl, data);
                    }
                    else
                    {
                        return new BinaryProcessor(url, actionOnNewUrl, data);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ErrorProcessor(url, actionOnNewUrl, ex);
            }
        }

        private static byte[] ReadBinaryResponse(WebResponse response)
        {
            using (var ms = new MemoryStream())
            {
                response.GetResponseStream().CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
