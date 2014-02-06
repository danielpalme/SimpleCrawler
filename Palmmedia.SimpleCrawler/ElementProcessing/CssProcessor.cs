
using System.Text;
using System.Text.RegularExpressions;
namespace Palmmedia.SimpleCrawler.ElementProcessing
{
    internal class CssProcessor : ElementProcessorBase
    {
        public CssProcessor(Url url, System.Action<Url> actionOnNewUrl, byte[] data)
            : base(url, actionOnNewUrl, data)
        {
        }

        public override void Process(string targetDirectory)
        {
            string css = this.Text;

            /* Replace relative urls, the following formats have to be allowed:
             * url(something.png)
             * url( something.png )
             * url("something.png")
             * url('something.png')
             */
            css = Regex.Replace(css, @"(?<start>url\(\s?\""?\'?)(?<url>.[^'""\s]*)(?<end>\""?\'?\s?\))", this.CreateRelativeUrl);

            this.Save(targetDirectory, Encoding.UTF8.GetBytes(css));
        }

        private string CreateRelativeUrl(Match match)
        {
            Url url = new Url(this.Url.Host, this.Url, match.Groups["url"].Value);
            this.ActionOnNewUrl(url);
            return match.Groups["start"].Value + url.RelativePath + match.Groups["end"].Value;
        }
    }
}
