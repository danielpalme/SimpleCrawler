﻿using System;
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
                throw new ArgumentNullException("host");
            }

            if (uri == null)
            {
                throw new ArgumentNullException("uri");
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

        public string Host { get; private set; }

        public string Uri { get; private set; }

        public string RelativePath
        {
            get
            {
                return this.GetRelativeUrl(this.relativeHost ?? this.Host);
            }
        }

        public string PathRelativeToHost
        {
            get
            {
                return this.GetRelativeUrl(this.Host);
            }
        }

        public bool EligibleForCrawling
        {
            get
            {
                return !this.invalidUrl
                    && !this.externalUrl;
            }
        }

        public override int GetHashCode()
        {
            return this.Uri.ToLowerInvariant().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Url other = obj as Url;

            return other != null && this.Uri.Equals(other.Uri, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            return this.Uri;
        }

        private string GetRelativeUrl(string relativeTo)
        {
            if (this.externalUrl || this.invalidUrl)
            {
                return this.Uri;
            }
            else
            {
                string relativePath = relativePath = new Uri(relativeTo).MakeRelativeUri(new Uri(this.Uri)).OriginalString;

                Match match = Regex.Match(relativePath, @"^(?<path>.*?)(?<extension>\.[^\.]*)?(?<querystring>\?.*)$");
                if (match.Success)
                {
                    relativePath = string.Format(
                        "{0}{1}{2}",
                        match.Groups["path"].Value,
                        Regex.Replace(match.Groups["querystring"].Value, @"[^\w]", m => "_"),
                        match.Groups["extension"].Value);
                }

                return relativePath;
            }
        }
    }
}