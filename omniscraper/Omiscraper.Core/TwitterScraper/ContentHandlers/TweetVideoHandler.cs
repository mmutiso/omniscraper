using Microsoft.Extensions.Logging;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class TweetVideoHandler: AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        ITwitterRepository twitterRepository;
        ILogger<TweetProcessingService> logger;
        TweetProcessorSettings settings;

        public TweetVideoHandler(IScraperRepository repository, ILogger<TweetProcessingService> logger,
            ITwitterRepository twitterRepository, TweetProcessorSettings settings)
        {
            scraperRepository = repository;
            this.logger = logger;
            this.twitterRepository = twitterRepository;
            this.settings = settings;
        }

        public override async Task HandleAsync(ContentRequestNotification notification)
        {
            RawTweet videoTweet = await twitterRepository.FindByIdAsync(notification.IdOfTweetBeingRepliedTo.Value);
            TweetNotification tweetNotification = new TweetNotification(videoTweet, notification.IdOfRequestingTweet, notification.RequestedBy);

            if (tweetNotification.HasVideo())
            {
                var video = tweetNotification.GetVideo();

                await scraperRepository.SaveTwitterVideoAsync(video);

                string response = video.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(video.RequestingTweetId, response);
                logger.LogInformation($"Sent back this response -> {response}");
            }
            else
            {
                logger.LogWarning($"The tweet being responded to didn't have a video -> {tweetNotification.TweetWithVideo.id}");
                await base.HandleAsync(notification);
            }            
        }
    }
}
