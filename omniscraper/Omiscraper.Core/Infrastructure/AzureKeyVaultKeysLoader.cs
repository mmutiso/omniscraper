using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Omniscraper.Core.Infrastructure
{
    public class AzureKeyVaultKeysLoader : ILoadApplicationKeys
    {
        ILogger<AzureKeyVaultKeysLoader> _logger;
        SecretClient _secretsClient;
        IConfiguration _configuration;

        private const string consumerKey = "ConsumerKeyName";
        private const string accessTokenSecret = "AccessTokenSecretKeyName";
        private const string accessToken = "AccessTokenKeyName";
        private const string consumerSecretKey = "ConsumerSecretKeyName";

        public AzureKeyVaultKeysLoader(SecretClient secretsClient, IConfiguration configuration, ILogger<AzureKeyVaultKeysLoader> logger)
        {
            _secretsClient = secretsClient?? throw new ArgumentNullException(nameof(secretsClient));
            _configuration = configuration??  throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string LoadByKeyName(string key)
        {
            KeyVaultSecret keyVaultSecret = _secretsClient.GetSecret(key);

            if (keyVaultSecret is null)
                throw new Exception($"{nameof(keyVaultSecret)} is null");

            _logger.LogInformation($"Reading KeyVault: {key} with value: {keyVaultSecret.Value}");

            return keyVaultSecret.Value;
        }

        public TwitterKeys LoadTwitterKeys()
        {
            var twitterKeysSection = _configuration.GetSection("KeyVault:AppKeys"); // This gets the name of the key in the Azure Key Vault

            var keys = new TwitterKeys
            {
                ConsumerKey = LoadByKeyName(twitterKeysSection[consumerKey]),
                ConsumerSecret = LoadByKeyName(twitterKeysSection[consumerSecretKey]),
                AccessToken = LoadByKeyName(twitterKeysSection[accessToken]),
                AccessTokenSecret = LoadByKeyName(twitterKeysSection[accessTokenSecret])
            };

            return keys;
        }
    }
}
