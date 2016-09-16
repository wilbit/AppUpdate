using NUnit.Framework;

namespace Wilbit.AppUpdate.Tests
{
    [TestFixture]
    public class XmlParserTests
    {
        private const string FeedSample =
            "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" +
            "<Deployment>\n" +
            "  <File name=\"FileName.exe\" version=\"1.0.0.0\">\n" +
            "    <Hash algo=\"AlgoName\" value=\"HashValue\" />\n" +
            "  </File>\n" +
            "</Deployment>";

        [Test]
        public void GetInfoFromXml_should_return()
        {
            var xmlParser = new XmlParser();

            var serverVersion = xmlParser.GetInfoFromXml(FeedSample);

            Assert.AreEqual("FileName.exe", serverVersion.FileName);
            Assert.IsNotNull(serverVersion.Version);
            Assert.AreEqual("1.0.0.0", serverVersion.Version.ToString());

            Assert.IsNotNull(serverVersion.Hash);
            Assert.AreEqual("AlgoName", serverVersion.Hash.Algo);
            Assert.AreEqual("HashValue", serverVersion.Hash.Value);
        }
    }
}