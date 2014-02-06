
using System.Text;
using System.Text.RegularExpressions;
namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal class HtmlProcessor : ElementProcessorBase
    {
        public HtmlProcessor(Url url, System.Action<Url> actionOnNewUrl, byte[] data)
            : base(url, actionOnNewUrl, data)
        {
        }

        public override void Process(string targetDirectory)
        {
            string html = this.Text;

            html = Regex.Replace(html, @"(?<start><a.*?href="")(?<url>.[^""]*)", this.CreateRelativeUrl);
            html = Regex.Replace(html, @"(?<start><link.*?href="")(?<url>.[^""]*)", this.CreateRelativeUrl);
            html = Regex.Replace(html, @"(?<start><script.*?src="")(?<url>.[^""]*)", this.CreateRelativeUrl);
            html = Regex.Replace(html, @"(?<start><img.*?src="")(?<url>.[^""]*)", this.CreateRelativeUrl);
            html = Regex.Replace(html, @"(?<start><input.*?src="")(?<url>.[^""]*)", this.CreateRelativeUrl);

            html = Regex.Replace(html, @"<base.*href="".[^""]*"".*/>", string.Empty);

            this.Save(targetDirectory, Encoding.UTF8.GetBytes(html));
        }

        private string CreateRelativeUrl(Match match)
        {
            Url url = new Url(this.Url.Host, match.Groups["url"].Value);
            this.ActionOnNewUrl(url);
            return match.Groups["start"] + url.RelativePath;
        }
    }
}
