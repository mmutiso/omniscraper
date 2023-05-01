using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public class VideosApiWrapper
    {
        private readonly TweetProcessorSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        ILogger<VideosApiWrapper> _logger;
        public VideosApiWrapper(IHttpClientFactory httpClientFactory, IOptions<TweetProcessorSettings> settings, ILogger<VideosApiWrapper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _logger = logger;  
        }

        public async Task<VideoResponseModel> GetVideoAsync(string tweetId)
        {
            using var httpClient = _httpClientFactory.CreateClient(_settings.VideoApiHttpClientName);
            string url = $"api/videos/{tweetId}";
            var response = await httpClient.GetAsync(url);

            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                
            };

            _logger.LogInformation(responseString);

            var videoObj = JsonSerializer.Deserialize<VideoResponseModel>(responseString, options);

            return videoObj;
        }
    }
}
