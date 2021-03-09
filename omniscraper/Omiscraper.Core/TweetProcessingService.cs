using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
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
        TwitterStreamModelFactory streamModelFactory;

        public TweetProcessingService(ILogger<TweetProcessingService> logger, TwitterContentHandlerFactory handlerFactory,
            TwitterStreamModelFactory streamModelFactory)
        {
            this.logger = logger;
            this.handlerFactory = handlerFactory;
            this.streamModelFactory = streamModelFactory;
        }

        public async Task ProcessTweetAsync(string tweetJsonString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //deserialize tweet.
            RawTweetv2 requestingTweet = JsonSerializer.Deserialize<RawTweetv2>(tweetJsonString);

            ITweetContentHandler handler = handlerFactory.BuildHandlerPipeline();
            TwitterStreamModel twitterStreamModel = streamModelFactory.GetTwitterStreamModel(requestingTweet);

            await handler.HandleAsync(twitterStreamModel, logger);
        }       
    }
}
