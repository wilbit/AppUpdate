using System;
using System.IO;
using System.Xml.Serialization;
using Wilbit.AppUpdate;
using Wilbit.AppUpdate.Helpers;
using Wilbit.FeedGenerator.Exceptions;

namespace Wilbit.FeedGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var argsHelper = ArgsHelper.Create(args);

                var fileName = Path.GetFileName(argsHelper.File);
                var fileVersion = argsHelper.Version;
                const string algo = "MD5";
                var hash = MD5Helper.GetHashForFile(argsHelper.File);
                var deployment = new XmlParser.DeploymentXmlClass
                {
                    File = new XmlParser.FileXmlClass
                    {
                        Name = fileName,
                        Version = fileVersion,
                        Hash = new XmlParser.HashXmlClass
                        {
                            Algo = algo,
                            Value = hash
                        }
                    }
                };

                using (var fileStream = new FileStream(argsHelper.Feed, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(XmlParser.DeploymentXmlClass));
                    serializer.Serialize(fileStream, deployment);
                }
                Console.WriteLine($"Feed \"{argsHelper.Feed}\" successfully generated");
            }
            catch (ArgsHelperException)
            {
                Console.WriteLine(ArgsHelper.GetHelp());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}