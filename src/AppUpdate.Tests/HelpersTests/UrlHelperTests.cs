using System;
using NUnit.Framework;
using Wilbit.AppUpdate.Helpers;

namespace Wilbit.AppUpdate.Tests.HelpersTests
{
    [TestFixture]
    public sealed class UrlHelperTests
    {
        [TestCase(null, @"http://www.example.com/Updates/AppNameSetup.exe")]
        [TestCase("", @"http://www.example.com/Updates/AppNameSetup.exe")]
        [TestCase(" ", @"http://www.example.com/Updates/AppNameSetup.exe")]
        [TestCase("paramName=paramValue", @"http://www.example.com/Updates/AppNameSetup.exe?paramName=paramValue")]
        public void CreateUrl_should_create_correct_uri(string @params, string expected)
        {
            var baseUri = new Uri(@"http://www.example.com/Updates/feed.xml");
            const string relativeUrl = "AppNameSetup.exe";

            var url = UrlHelper.CreateUrl(baseUri, relativeUrl, @params);

            Assert.AreEqual(expected, url.AbsoluteUri);
        }
    }
}