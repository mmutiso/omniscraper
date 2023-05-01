using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Omniscraper.Core.TwitterScraper.Entities.v2;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class SeenVideoHandler : AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        TweetProcessorSettings settings;
        ITwitterRepository twitterRepository;
        OpenAICompleter openAICompleter;

        public SeenVideoHandler(IScraperRepository scraperRepository, 
            IOptions<TweetProcessorSettings> options, ITwitterRepository twitterRepository, OpenAICompleter openAICompleter)
        {
            this.scraperRepository = scraperRepository;
            settings = options.Value;
            this.twitterRepository = twitterRepository;
            this.openAICompleter = openAICompleter;
        }
        public override async Task HandleAsync<T>(StreamedTweetContent notification, ILogger<T> logger)
        {
            TwitterVideo twitterVideo;
            bool exists = scraperRepository.GetIfVideoExists(notification.TweetRepliedToId, out twitterVideo);
            if (exists)
            {
                logger.LogInformation($"This tweet existed {notification.TweetRepliedToId}.");

                var request = new TwitterVideoRequest(notification.RequestingTweetId, twitterVideo.Id, notification.RequestedBy);

                await scraperRepository.CaptureTwitterRequestAsync(request);

                var choice = await openAICompleter.GetOpenAIReponseAsync();

                string response = twitterVideo.GetResponseContent(settings.BaseUrl, request.RequestedBy, choice);
                //send back response
                await twitterRepository.ReplyToTweetAsync(notification.RequestingTweetId.ToString(), response);
                return;
            }
            else
            {
                await base.HandleAsync(notification, logger);
            }
        }
    }
}
