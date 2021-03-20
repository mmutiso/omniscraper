using Microsoft.Extensions.Logging;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class ThreadHandler: AbstractTweetContentHandler
    {
        IConversationBuilder conversationBuilder;
        ITwitterRepository twitterRepository;
        IScraperRepository scraperRepository;
        TweetProcessorSettings settings;

        public ThreadHandler(IConversationBuilder conversationBuilder, ITwitterRepository twitterRepository,
            IScraperRepository scraperRepository, TweetProcessorSettings settings)
        {
            this.conversationBuilder = conversationBuilder;
            this.twitterRepository = twitterRepository;
            this.scraperRepository = scraperRepository;
            this.settings = settings;
        }
        public override async Task HandleAsync<T>(ContentRequestNotification notification, ILogger<T> logger)
        {
            if (!notification.IdOfTweetBeingRepliedTo.HasValue)
            {
                logger.LogWarning($"Attempted to build a thread from a request that wasn't a reply {notification.IdOfRequestingTweet}");
                return;
            }

            RawTweet tweetBeingRepliedTo = await twitterRepository.FindByIdAsync(notification.IdOfTweetBeingRepliedTo.Value);

            BuildConversationResult result = await conversationBuilder.BuildAsync(tweetBeingRepliedTo.user.id, tweetBeingRepliedTo.id);

            if (result.Success)
            {
                var thread = result.TwitterConversation.CreateThreadFromConversation();
                string response = thread.GetThreadResponse(settings.BaseUrl, notification.RequestedBy);
                await scraperRepository.SaveThreadAsync(thread);
                logger.LogInformation($"Saved a thread with ID {thread.Id}");


                await twitterRepository.ReplyToTweetAsync(notification.IdOfRequestingTweet, response);
                logger.LogInformation($"Send back this response ->{response}");
            }
            else
            {
                logger.LogWarning($"This was not a thread. Stopping further processing. Requesting tweet Id {notification.IdOfRequestingTweet}");
            }
        }
    }
}
