using LinqToTwitter.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace Omniscraper.Api.Models
{
    public class TwitterAuthorizationService
    {
        private const string ConsumerKeyName = "OMNISCRAPER_CONSUMERKEY";
        private const string ConsumerSecretName = "OMNISCRAPER_CONSUMERSECRET";
        private const string AccessTokenName = "OMNISCRAPER_ACCESSTOKEN";
        private const string AccessTokenSecretName = "OMNISCRAPER_ACCESSTOKENSECRET";

        public static IAuthorizer Authorize()
        {
            var keys = GetKeys();

            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = keys.ConsumerKey,
                    ConsumerSecret = keys.ConsumerSecret
                }
            };
            return auth;
        }

        public static TwitterKeys GetKeys()
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

        public static string GetValue(string key)
        {
            string envValue = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(envValue))
                Console.WriteLine($"No value found for Variable: {key} ");

            return envValue;
        }
    }
}
