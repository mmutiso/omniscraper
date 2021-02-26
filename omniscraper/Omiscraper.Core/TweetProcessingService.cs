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
using Omniscraper.Core.TwitterScraper.ContentHandlers;

namespace Omniscraper.Core
{
    public class TweetProcessingService
    {
       
        ILogger<TweetProcessingService> logger;
        TwitterContentHandlerFactory handlerFactory;

        public TweetProcessingService(ILogger<TweetProcessingService> logger, TwitterContentHandlerFactory handlerFactory)
        {
            this.logger = logger;
            this.handlerFactory = handlerFactory;
        }

        public async Task ProcessTweetAsync(string tweetJsonString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //deserialize tweet.
            RawTweet requestingTweet = DeserializeTweet(tweetJsonString);

            ITweetContentHandler handler = handlerFactory.BuildHandlerPipeline();
            var requestNotification = requestingTweet.CreateRequestNotification();

            await handler.HandleAsync(requestNotification, logger);
        }       

        private RawTweet DeserializeTweet(string tweetJsonString)
        {
            var tweet = JsonConvert.DeserializeObject<RawTweet>(tweetJsonString);

            return tweet;
        }
    }
}
