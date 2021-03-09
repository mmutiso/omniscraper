using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class SeenVideoHandler : AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        TweetProcessorSettings settings;
        ITwitterRepository twitterRepository;

        public SeenVideoHandler(IScraperRepository scraperRepository, 
            IOptions<TweetProcessorSettings> options, ITwitterRepository twitterRepository)
        {
            this.scraperRepository = scraperRepository;
            settings = options.Value;
            this.twitterRepository = twitterRepository;
        }
        public override async Task HandleAsync<T>(TwitterStreamModel twitterStreamModel, ILogger<T> logger)
        {
            TwitterVideo twitterVideo;
            bool exists = scraperRepository.GetIfVideoExists(twitterStreamModel.IdIfTweetBeingRepliedTo.Value, out twitterVideo);
            if (exists)
            {
                logger.LogInformation($"This tweet existed {twitterStreamModel.IdIfTweetBeingRepliedTo.Value}.");

                var mutatedVideo = twitterVideo.CreateForNewTweet(twitterStreamModel.TweetId, twitterStreamModel.RequesterUserName);
                await scraperRepository.SaveTwitterVideoAsync(mutatedVideo);

                string response = mutatedVideo.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(mutatedVideo.RequestingTweetId, response);
                return;
            }
            else
            {
                await base.HandleAsync(twitterStreamModel, logger);
            }
        }
    }
}
