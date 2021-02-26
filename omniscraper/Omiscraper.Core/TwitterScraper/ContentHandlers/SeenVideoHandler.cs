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
        ILogger<SeenVideoHandler> logger;
        TweetProcessorSettings settings;
        ITwitterRepository twitterRepository;

        public SeenVideoHandler(IScraperRepository scraperRepository, ILogger<SeenVideoHandler> logger,
            IOptions<TweetProcessorSettings> options, ITwitterRepository twitterRepository)
        {
            this.scraperRepository = scraperRepository;
            this.logger = logger;
            settings = options.Value;
            this.twitterRepository = twitterRepository;
        }
        public override async Task HandleAsync(ContentRequestNotification notification)
        {
            TwitterVideo twitterVideo;
            bool exists = scraperRepository.GetIfVideoExists(notification.IdOfTweetBeingRepliedTo.Value, out twitterVideo);
            if (exists)
            {
                logger.LogInformation($"This tweet existed {notification.IdOfTweetBeingRepliedTo.Value}.");

                var mutatedVideo = twitterVideo.CreateForNewTweet(notification.IdOfRequestingTweet, notification.RequestedBy);
                await scraperRepository.SaveTwitterVideoAsync(mutatedVideo);

                string response = mutatedVideo.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(mutatedVideo.RequestingTweetId, response);
                return;
            }
            else
            {
                await base.HandleAsync(notification);
            }
        }
    }
}
