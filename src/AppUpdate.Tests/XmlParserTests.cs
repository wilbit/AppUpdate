using NUnit.Framework;

namespace Wilbit.AppUpdate.Tests
{
    [TestFixture]
    public class XmlParserTests
    {
        private const string FeedSample =
            "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
            "<Deployment>\n" +
            "  <File Name=\"FileName.exe\" Version=\"1.0.0.0\" HashAlgo=\"MD5\" HashValue=\"TestValue\" />\n" +
            "</Deployment>";

        [Test]
        public void GetInfoFromXml_should_return([Values(FeedSample)] string xml)
        {
            var xmlParser = new XmlParser();

            var serverVersion = xmlParser.GetInfoFromXml(xml);

            Assert.AreEqual("FileName.exe", serverVersion.FileName);
            Assert.IsNotNull(serverVersion.Version);
            Assert.AreEqual("1.0.0.0", serverVersion.Version.ToString());
            Assert.IsNotNull(serverVersion.Hash);
            Assert.AreEqual("MD5", serverVersion.Hash.Algo);
            Assert.AreEqual("TestValue", serverVersion.Hash.Value);
        }
    }
}