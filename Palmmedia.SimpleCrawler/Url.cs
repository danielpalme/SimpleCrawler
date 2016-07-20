using System;
using System.Text.RegularExpressions;

namespace Palmmedia.SimpleCrawler
{
    internal class Url
    {
        private readonly bool externalUrl, invalidUrl;

        private readonly string relativeHost;

        public Url(string host, string uri)
            : this(host, null, uri)
        {
        }

        public Url(string host, Url relativeTo, string uri)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            this.Host = host.TrimEnd('/') + "/";
            this.relativeHost = relativeTo == null ? null : relativeTo.Uri.Substring(0, relativeTo.Uri.LastIndexOf('/') + 1);

            if (uri.StartsWith(this.Host, StringComparison.OrdinalIgnoreCase))
            {
                this.Uri = uri;
            }
            else if (uri.StartsWith(System.Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                || uri.StartsWith(System.Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
                || uri.StartsWith(System.Uri.UriSchemeMailto, StringComparison.OrdinalIgnoreCase)
                || uri.StartsWith(System.Uri.UriSchemeFtp, StringComparison.OrdinalIgnoreCase))
            {
                this.externalUrl = true;
                this.Uri = uri;
            }
            else
            {

                if (relativeTo != null && uri.StartsWith("/"))
                {
                    this.Uri = this.Host + uri.TrimStart('/');
                }
                else
                {
                    System.Uri result = null;

                    string currentHost = relativeTo == null ? this.Host : relativeHost;

                    if (System.Uri.TryCreate(new Uri(currentHost), uri, out result))
                    {
                        this.Uri = result.AbsoluteUri;
                    }
                    else
                    {
                        this.invalidUrl = true;
                    }

                }

            }
        }

        public string Host { get; }

        public string Uri { get; }

        public string RelativePath => this.GetRelativeUrl(this.relativeHost ?? this.Host, true);

        public string PathRelativeToHost => this.GetRelativeUrl(this.Host, false);

        public bool EligibleForCrawling => !this.invalidUrl
    && !this.externalUrl;

        public override int GetHashCode() => this.Uri.ToLowerInvariant().GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Url other = obj as Url;

            return other != null && this.Uri.Equals(other.Uri, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString() => this.Uri;

        private string GetRelativeUrl(string relativeTo, bool includeAnchor)
        {
            if (this.externalUrl || this.invalidUrl)
            {
                return this.Uri;
            }
            else
            {
                string relativePath = relativePath = new Uri(relativeTo).MakeRelativeUri(new Uri(this.Uri)).OriginalString;

                Match match = Regex.Match(relativePath, @"^(?<path>.*?)(?<extension>\.[^\.]*?)?(?<querystring>\?.*?)?(?<anchor>#.*)?$");
                if (match.Success)
                {
                    relativePath = string.Format(
                        "{0}{1}{2}{3}",
                        match.Groups["path"].Value,
                        Regex.Replace(match.Groups["querystring"].Value, @"[^\w]", m => "_"),
                        match.Groups["extension"].Value,
                        includeAnchor ? match.Groups["anchor"].Value : string.Empty);
                }

                return relativePath;
            }
        }
    }
}
