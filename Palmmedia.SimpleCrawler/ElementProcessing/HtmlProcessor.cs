
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
            string urlText = match.Groups["url"].Value;

            if (urlText.StartsWith("mailto:", System.StringComparison.OrdinalIgnoreCase)
                || urlText.StartsWith("tel:", System.StringComparison.OrdinalIgnoreCase))
            {
                return match.Groups[0].Value;
            }
            else
            {
                Url url = new Url(this.Url.Host, urlText);
                this.ActionOnNewUrl(url);
                return match.Groups["start"] + url.RelativePath;
            }
        }
    }
}
