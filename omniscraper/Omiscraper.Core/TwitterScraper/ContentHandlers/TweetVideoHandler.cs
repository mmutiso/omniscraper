using Microsoft.Extensions.Logging;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Omniscraper.Core.Infrastructure;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class TweetVideoHandler: AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        ITwitterRepository twitterRepository;
        TweetProcessorSettings settings;
        OpenAICompleter openAICompleter;

        public TweetVideoHandler(IScraperRepository repository, 
            ITwitterRepository twitterRepository,
            IOptions<TweetProcessorSettings> settings, OpenAICompleter openAICompleter)
        {
            scraperRepository = repository;
            this.twitterRepository = twitterRepository;
            this.settings = settings.Value;
            this.openAICompleter = openAICompleter;
        }

        public override async Task HandleAsync<T>(ContentRequestNotification notification, ILogger<T> logger)
        {
            RawTweet videoTweet = await twitterRepository.FindByIdAsync(notification.IdOfTweetBeingRepliedTo.Value);
            TweetNotification tweetNotification = new TweetNotification(videoTweet, notification.IdOfRequestingTweet, notification.RequestedBy);

            if (tweetNotification.HasVideo())
            {
                var video = tweetNotification.GetVideo();
                var request = tweetNotification.GetVideoRequest(video.Id);

                await scraperRepository.CaptureTwitterVideoAndRequestAsync(request, video);

                var choice = await openAICompleter.GetOpenAIReponseAsync();

                string response = video.GetResponseContent(settings.BaseUrl, request.RequestedBy, choice);

                //send back response
                await twitterRepository.ReplyToTweetAsync(request.RequestingTweetId, response);
                logger.LogInformation($"Sent back this response -> {response}");
            }
            else
            {
                logger.LogWarning($"The tweet being responded to didn't have a video -> {tweetNotification.TweetWithVideo.id}");
                await base.HandleAsync(notification, logger);
            }            
        }
    }
}
