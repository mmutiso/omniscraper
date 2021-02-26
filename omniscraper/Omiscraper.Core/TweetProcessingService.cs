using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Omniscraper.Core.TwitterScraper.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Omniscraper.Core
{
    public class TweetProcessingService
    {
        ITwitterRepository twitterRepository;
        IScraperRepository scraperRepository;
        ILogger<TweetProcessingService> logger;
        TweetProcessorSettings settings; 

        public TweetProcessingService(ITwitterRepository twitterRepository, IScraperRepository scraperRepository,
            ILogger<TweetProcessingService> logger, IOptions<TweetProcessorSettings> options)
        {
            this.twitterRepository = twitterRepository;
            this.scraperRepository = scraperRepository;
            this.logger = logger;
            settings = options.Value;
        }

        public async Task ProcessTweetAsync(string tweetJsonString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //deserialize tweet.
            RawTweet requestingTweet = DeserializeTweet(tweetJsonString);
            




        }

       

        private RawTweet DeserializeTweet(string tweetJsonString)
        {
            var tweet = JsonConvert.DeserializeObject<RawTweet>(tweetJsonString);

            return tweet;
        }
    }
}
