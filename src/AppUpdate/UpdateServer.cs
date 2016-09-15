using System;
using System.Threading;
using Wilbit.AppUpdate.Exceptions;

namespace Wilbit.AppUpdate
{
    internal sealed class UpdateServer
    {
        private readonly XmlParser _xmlParser = new XmlParser();

        public ServerVersionInfo GetServerVersionInfo(Feed feed, TimeSpan timeout)
        {
            if (feed == null) throw new ArgumentNullException(nameof(feed));

            Exception lastError = null;

            for (var i = 0; i < 3; i++)
            {
                try
                {
                    var xml = feed.GetXml(timeout);
                    var serverVersionInfo = _xmlParser.GetInfoFromXml(xml);
                    serverVersionInfo.Feed = feed;

                    return serverVersionInfo;
                }
                catch (Exception e)
                {
                    lastError = e;
                    Thread.Sleep(300);
                }
            }

            throw new UpdateServerException("Failed to get server version info by url", lastError);
        }
    }
}