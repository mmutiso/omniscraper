using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.TwitterScraper.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Omniscraper.Core.TwitterScraper.ContentHandlers;
using Omniscraper.Core.TwitterScraper.Entities.v2;
using System.Text.Json;

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

        public async Task ProcessTweetAsync(string v2tweetJsonString)
        {
            StreamedTweetContent streamedContent = DeserializeTweet(v2tweetJsonString);

            ITweetContentHandler handler = handlerFactory.BuildHandlerPipeline();

            await handler.HandleAsync(streamedContent, logger);
        }       

        private StreamedTweetContent DeserializeTweet(string tweetJsonString)
        {
            var tweet = JsonSerializer.Deserialize<StreamedTweetContent>(tweetJsonString);

            return tweet;
        }
    }
}
