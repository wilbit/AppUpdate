using System;
using System.IO;
using System.Xml.Serialization;

namespace Wilbit.AppUpdate
{
    public sealed class XmlParser
    {
        public ServerVersionInfo GetInfoFromXml(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(DeploymentXml));
                FileXml fileXml;
                using (var stringReader = new StringReader(xml))
                {
                    var xmlClass = (DeploymentXml) serializer.Deserialize(stringReader);
                    fileXml = xmlClass.File;
                }

                var result = new ServerVersionInfo
                (
                    fileName: fileXml.Name,
                    version: new Version(fileXml.Version),
                    hash: new HashInfo(algo: fileXml.HashAlgo, value: fileXml.HashValue)
                );

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed parse xml [{xml}]", e);
            }
        }

        [XmlRoot("Deployment")]
        public class DeploymentXml
        {
            [XmlElement("File")]
            public FileXml File { get; set; }
        }

        [XmlRoot("File")]
        public class FileXml
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }

            [XmlAttribute("Version")]
            public string Version { get; set; }

            [XmlAttribute("HashAlgo")]
            public string HashAlgo { get; set; }

            [XmlAttribute("HashValue")]
            public string HashValue { get; set; }
        }
    }
}