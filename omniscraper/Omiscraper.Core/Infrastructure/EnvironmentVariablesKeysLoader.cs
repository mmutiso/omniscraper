using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Omniscraper.Core.Infrastructure
{
    public class EnvironmentVariablesKeysLoader : ILoadApplicationCredentials
    {
        private const string ConsumerKeyName = "OMNISCRAPER_CONSUMERKEY";
        private const string ConsumerSecretName = "OMNISCRAPER_CONSUMERSECRET";
        private const string AccessTokenName = "OMNISCRAPER_ACCESSTOKEN";
        private const string AccessTokenSecretName = "OMNISCRAPER_ACCESSTOKENSECRET";

        ILogger<EnvironmentVariablesKeysLoader> logger;
        
        public EnvironmentVariablesKeysLoader(ILogger<EnvironmentVariablesKeysLoader> logger)
        {
            foreach (var variable in Environment.GetEnvironmentVariables())
            {
                Console.WriteLine($"spitting out: {JsonSerializer.Serialize(variable)}");
            }
            this.logger = logger;
        }
        private string GetValue(string key)
        {
            string envValue = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(envValue))
                logger.LogWarning($"No value found for Variable: {key} ");

            return envValue;
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
