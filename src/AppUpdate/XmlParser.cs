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
                var serializer = new XmlSerializer(typeof(DeploymentXmlClass));
                DeploymentXmlClass deployment;
                using (var stringReader = new StringReader(xml))
                {
                    deployment = (DeploymentXmlClass) serializer.Deserialize(stringReader);
                }

                var result = new ServerVersionInfo(
                    fileName: deployment.File?.Name ?? string.Empty,
                    version: new Version(deployment.File?.Version ?? "0.0.0.0"),
                    hash: new HashInfo(
                        algo: deployment.File?.Hash?.Algo ?? string.Empty,
                        value: deployment.File?.Hash?.Value ?? string.Empty),
                    comment: deployment.Comment ?? string.Empty
                    );

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed parse xml [{xml}]", e);
            }
        }

        [XmlRoot("Deployment")]
        public class DeploymentXmlClass
        {
            [XmlElement("File")]
            public FileXmlClass File { get; set; }

            [XmlElement("Comment")]
            public string Comment { get; set; }
        }

        [XmlRoot("File")]
        public class FileXmlClass
        {
            [XmlAttribute("name")]
            public string Name { get; set; }

            [XmlAttribute("version")]
            public string Version { get; set; }

            [XmlElement("Hash")]
            public HashXmlClass Hash { get; set; }
        }

        [XmlRoot("Hash")]
        public class HashXmlClass
        {
            [XmlAttribute("algo")]
            public string Algo { get; set; }

            [XmlAttribute("value")]
            public string Value { get; set; }
        }
    }
}