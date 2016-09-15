using System;

namespace Wilbit.AppUpdate.Helpers
{
    public sealed class UrlHelper
    {
        public static Uri CreateUrl(Uri baseUri, string relativeUrl, string @params)
        {
            return string.IsNullOrWhiteSpace(@params)
                ? new Uri(baseUri, relativeUrl)
                : new Uri(baseUri, $"{relativeUrl}?{@params}");
        }
    }
}