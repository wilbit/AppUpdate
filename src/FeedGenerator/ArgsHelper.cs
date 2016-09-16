using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wilbit.FeedGenerator.Exceptions;

namespace Wilbit.FeedGenerator
{
    public sealed class ArgsHelper
    {
        private static readonly ICollection<string> AvailableKeys = new[]
        {
            FileKey,
            VersionKey,
            FeedKey
        };

        private const string FileKey = "file";
        private const string VersionKey = "version";
        private const string FeedKey = "feed";

        private IDictionary<string, string> _argsDictionary;

        public string File => _argsDictionary[FileKey];

        public string Version => _argsDictionary[VersionKey];

        public string Feed => _argsDictionary[FeedKey];

        private ArgsHelper()
        {
        }

        public static ArgsHelper Create(ICollection<string> args)
        {
            var dictionary = args
                .Select(x => x.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(x => x.Length == 2)
                .ToDictionary(x => x[0].ToLowerInvariant(), x => x[1]);
            if (dictionary.Count != args.Count || dictionary.Count != AvailableKeys.Count || dictionary.Any(x => AvailableKeys.All(y => !y.Equals(x.Key, StringComparison.Ordinal))))
                throw new ArgsHelperException($"Invalid args: {string.Join(" ", args)}");

            return new ArgsHelper
            {
                _argsDictionary = dictionary
            };
        }

        public static string GetHelp()
        {
            return $"Usage:\r\n    {Path.GetFileName(typeof(ArgsHelper).Assembly.Location)} {string.Join(" ", AvailableKeys.Select(x => $"{x}=<{x}>"))}";
        }
    }
}