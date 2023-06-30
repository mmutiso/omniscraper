using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using LinqToTwitter;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using Microsoft.Extensions.Options;

namespace Omniscraper.Core.Infrastructure
{
	public class OpenAICompleter
    {
		ILogger<OpenAICompleter> logger;
		IHttpClientFactory httpClientFactory;
        private readonly TweetProcessorSettings _settings;
        OpenAISettings openAISettings;

        public OpenAICompleter(
            ILogger<OpenAICompleter> logger,
            IHttpClientFactory httpClientFactory,
            IOptions<TweetProcessorSettings> settings,
            OpenAISettings openAISettings)
		{
			this.logger = logger;
			this.httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            this.openAISettings = openAISettings;
		}

		public async Task<string> GetOpenAICompletionAsync()
		{
            using var httpClient = httpClientFactory.CreateClient(_settings.OpenAIHttpClientName);

            var requestBody = openAISettings.Prompt;

            var data = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var message = await httpClient.PostAsync("completions", data);

			if(message.IsSuccessStatusCode)
			{
                var response = await message.Content.ReadAsStringAsync();

                logger.LogInformation(response);

				var deserializedRes = JsonSerializer.Deserialize<OpenAIResponse>(response);

                if(deserializedRes?.Choices.Count > 0)
                {
                    var choice = deserializedRes.Choices[0];

                    if (choice.Finish_Reason == "stop")
                    {
                        return choice.Text.Trim();
                    }
                }
            }

            return string.Empty;
        }
    }
}

