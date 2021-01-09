using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Infrastructure
{
    public class EnvironmentVariablesKeysLoader : ILoadApplicationCredentials
    {
        private const string ConsumerKeyName = "OmniScraper_ConsumerKey";
        private const string ConsumerSecretName = "OmniScraper_ConsumerSecret";
        private const string AccessTokenName = "OmniScraper_AccessToken";
        private const string AccessTokenSecretName = "OmniScraper_AccessTokenSecret";

        private string GetValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }

        public TwitterKeys Load()
        {
            var keys = new TwitterKeys
            {
                ConsumerKey = GetValue(ConsumerKeyName),
                ConsumerSecret = GetValue(ConsumerSecretName),
                AccessToken = GetValue(AccessTokenName),
                AccessTokenSecret = GetValue(AccessTokenSecretName)
            };

            return keys;
        }
    }
}
