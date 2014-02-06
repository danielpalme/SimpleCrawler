using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Palmmedia.SimpleCrawler.Test
{
    [TestClass]
    public class UrlTest
    {
        [TestMethod]
        public void Constuctor_AbsoluteUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url("http://localhost/subdirectory", "http://localhost/subdirectory/index.hmtl");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://localhost/subdirectory/index.hmtl", url.Uri);
        }

        [TestMethod]
        public void Constuctor_ExternalUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url("http://localhost/subdirectory", "http://www.somethingelse.xyz");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://www.somethingelse.xyz", url.Uri);
        }

        [TestMethod]
        public void Constuctor_RelativeUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url("http://localhost/subdirectory", "test/index.hmtl");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://localhost/subdirectory/test/index.hmtl", url.Uri);
        }

        [TestMethod]
        public void Constuctor_RootedUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url("http://localhost/subdirectory", "/test/index.hmtl");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://localhost/test/index.hmtl", url.Uri);
        }

        [TestMethod]
        public void Constuctor_RelativeToStylesheetUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url(
                "http://localhost/subdirectory", 
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "../Images/test.png");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://localhost/subdirectory/Images/test.png", url.Uri);
        }

        [TestMethod]
        public void Constuctor_RootedUrlRelativeToStylesheetUrl_PropertiesCorrectlyAssigned()
        {
            var url = new Url(
                "http://localhost/subdirectory",
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "/Images/test.png");

            Assert.AreEqual("http://localhost/subdirectory/", url.Host);
            Assert.AreEqual("http://localhost/subdirectory/Images/test.png", url.Uri);
        }

        [TestMethod]
        public void EligibleForCrawling_LocalUrl_True()
        {
            var url = new Url("http://localhost/subdirectory", "http://localhost/subdirectory/index.hmtl");

            Assert.IsTrue(url.EligibleForCrawling);
        }

        [TestMethod]
        public void EligibleForCrawling_ExternalUrl_False()
        {
            var url = new Url("http://localhost/subdirectory", "http://www.somethingelse.xyz");

            Assert.IsFalse(url.EligibleForCrawling);
        }

        [TestMethod]
        public void EligibleForCrawling_MailtoUrl_False()
        {
            var url = new Url("http://localhost/subdirectory", "mailto:test@somethingelse.xyz");

            Assert.IsFalse(url.EligibleForCrawling);
        }

        [TestMethod]
        public void Equals_SameObject_True()
        {
            var url = new Url("http://localhost/subdirectory", "http://localhost/subdirectory/index.hmtl");

            Assert.IsTrue(url.Equals(url));
        }

        [TestMethod]
        public void Equals_SameUrl_True()
        {
            var url1 = new Url("http://localhost/subdirectory", "http://localhost/subdirectory/index.hmtl");
            var url2 = new Url("http://localhost/subdirectory", "index.hmtl");

            Assert.IsTrue(url1.Equals(url2));
        }

        [TestMethod]
        public void Equals_Null_False()
        {
            var url = new Url("http://localhost/subdirectory", "http://localhost/subdirectory/index.hmtl");

            Assert.IsFalse(url.Equals(null));
        }

        [TestMethod]
        public void RelativePath_RelativeUrl_CorrectPathReturned()
        {
            var url = new Url("http://localhost/subdirectory", "Images/test.png");

            Assert.AreEqual("Images/test.png", url.RelativePath);
        }

        [TestMethod]
        public void RelativePath_RelativeToStylesheetUrl_CorrectPathReturned()
        {
            var url = new Url(
                "http://localhost/subdirectory",
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "../Images/test.png");

            Assert.AreEqual("../Images/test.png", url.RelativePath);
        }

        [TestMethod]
        public void RelativePath_RelativeUrlWithQueryString_CorrectPathReturned()
        {
            var url = new Url("http://localhost/subdirectory", "Images/test.png?x1=abc&x2=cba()");

            Assert.AreEqual("Images/test_x1_abc_x2_cba__.png", url.RelativePath);
        }

        [TestMethod]
        public void RelativePath_RelativeToStylesheetUrlWithQueryString_CorrectPathReturned()
        {
            var url = new Url(
                "http://localhost/subdirectory",
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "../Images/test.png?x1=abc&x2=cba()");

            Assert.AreEqual("../Images/test_x1_abc_x2_cba__.png", url.RelativePath);
        }

        [TestMethod]
        public void PathRelativeToHost_RelativeUrl_CorrectPathReturned()
        {
            var url = new Url("http://localhost/subdirectory", "Images/test.png");

            Assert.AreEqual("Images/test.png", url.PathRelativeToHost);
        }

        [TestMethod]
        public void PathRelativeToHost_RelativeToStylesheetUrl_CorrectPathReturned()
        {
            var url = new Url(
                "http://localhost/subdirectory",
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "../Images/test.png");

            Assert.AreEqual("Images/test.png", url.PathRelativeToHost);
        }

        [TestMethod]
        public void PathRelativeToHost_RelativeUrlWithQueryString_CorrectPathReturned()
        {
            var url = new Url("http://localhost/subdirectory", "Images/test.png?x1=abc&x2=cba()");

            Assert.AreEqual("Images/test_x1_abc_x2_cba__.png", url.PathRelativeToHost);
        }

        [TestMethod]
        public void PathRelativeToHost_RelativeToStylesheetUrlWithQueryString_CorrectPathReturned()
        {
            var url = new Url(
                "http://localhost/subdirectory",
                new Url("http://localhost/subdirectory", "http://localhost/subdirectory/Content/custom.css"),
                "../Images/test.png?x1=abc&x2=cba()");

            Assert.AreEqual("Images/test_x1_abc_x2_cba__.png", url.PathRelativeToHost);
        }
    }
}
