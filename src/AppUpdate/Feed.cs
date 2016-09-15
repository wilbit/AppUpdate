using System;
using System.IO;
using System.Net;
using Wilbit.AppUpdate.Exceptions;

namespace Wilbit.AppUpdate
{
    public sealed class Feed
    {
        public Feed(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            Uri = uri;
        }

        public Uri Uri { get; }

        public string GetXml(TimeSpan timeout)
        {
            try
            {
                TryResolvingHostForSpeedup(Uri);

                var data = string.Empty;
                var request = (HttpWebRequest) WebRequest.Create(Uri);
                request.Host = Uri.Host;
                request.Method = "GET";
                request.Timeout = (int)timeout.TotalMilliseconds;

                using (var response = request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    if (stream == null)
                    {
                        return data;
                    }

                    using (var reader = new StreamReader(stream, true))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                return data;
            }
            catch (WebException e)
            {
                throw new FeedSourceException($"Failed to request {Uri}", e);
            }
        }

        private static void TryResolvingHostForSpeedup(Uri uri)
        {
            try
            {
                Dns.GetHostEntry(uri.Host);
            }
            catch (Exception)
            {
                throw new WebException($"Failed to resolve {uri.Host}. Check your connectivity.", WebExceptionStatus.ConnectFailure);
            }
        }
    }
}