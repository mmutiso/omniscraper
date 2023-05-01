using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using LinqToTwitter;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.Core.Infrastructure
{
	public class OpenAICompleter
    {
		IConfiguration configuration;
		ILogger<OpenAICompleter> logger;
        SecretClient secretsClient;
		//IHttpClientFactory httpClientFactory;

		public OpenAICompleter(IConfiguration configuration, ILogger<OpenAICompleter> logger, SecretClient secretsClient)
		{
			this.configuration = configuration;
			this.logger = logger;
			this.secretsClient = secretsClient;
			//this.httpClientFactory = httpClientFactory;
		}

		public async Task<string> GetOpenAIReponseAsync()
		{
            HttpClient httpClient = new HttpClient();

			var key = "OpenaiRequest";
            var openAIRequest = "{\n    \"model\": \"text-davinci-003\",\n    \"prompt\": \"Question: Tell me a fun fact that I can share with my friends? \\nAnswer:\",\n    \"temperature\": 0.5,\n    \"max_tokens\": 50\n  }";

            //var openaiRequest = Environment.GetEnvironmentVariable(key);

            //if (string.IsNullOrWhiteSpace(""))
            //{
            //	Console.WriteLine($"The item with the key {key} does not exist!");
            //	return string.Empty;
            //}

            string openaiKey = configuration.GetSection("KeyVault:OpenAIKey").Value;
            KeyVaultSecret keyVaultSecret = secretsClient.GetSecret(openaiKey);

			if (keyVaultSecret is null)
				return string.Empty;

			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{keyVaultSecret.Value}");

            var data = new StringContent(openAIRequest, Encoding.UTF8, "application/json");

            var message = await httpClient.PostAsync("https://api.openai.com/v1/completions", data);

			if(message.IsSuccessStatusCode)
			{
                var response = await message.Content.ReadAsStringAsync();

				var choice = JsonConvert.DeserializeObject<OpenAIResponse>(response).Choices.First();

				if(choice.Finish_Reason != "stop")
				{
                    return string.Empty;
                }

				return $"FUN FACT: {choice.Text.Trim()}";
            }

            return string.Empty;
        }
    }
}

