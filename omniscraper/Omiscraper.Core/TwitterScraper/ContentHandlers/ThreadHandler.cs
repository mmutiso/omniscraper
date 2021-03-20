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

            TwitterThread thread;
            bool exists = scraperRepository.GetThreadIfExists(notification.IdOfTweetBeingRepliedTo.Value, out thread);

            if (exists)
            {
                var request = new TwitterThreadRequest(notification.IdOfRequestingTweet, thread.Id, notification.RequestedBy);
                await scraperRepository.SaveAsync(request);
                logger.LogInformation($"Thread exists. Saving request by tweet id {notification.IdOfRequestingTweet}");
                await SendResponse(thread, notification, logger);
                return;
            }

            RawTweet tweetBeingRepliedTo = await twitterRepository.FindByIdAsync(notification.IdOfTweetBeingRepliedTo.Value);

            BuildConversationResult result = await conversationBuilder.BuildAsync(tweetBeingRepliedTo.user.id, tweetBeingRepliedTo.id);

            if (result.Success)
            {                
                thread = result.TwitterConversation.CreateThreadFromConversation();
                var request = new TwitterThreadRequest(notification.IdOfRequestingTweet, thread.Id, notification.RequestedBy);
                await scraperRepository.SaveAsync(thread, request);

                logger.LogInformation($"Saved a thread with ID {thread.Id} and request");

                await SendResponse(thread, notification, logger);                
            }
            else
            {
                logger.LogWarning($"This was not a thread. Stopping further processing. Requesting tweet Id {notification.IdOfRequestingTweet}");
            }
        }

        async Task SendResponse<T>(TwitterThread thread, ContentRequestNotification notification, ILogger<T> logger)
        {
            string response = thread.GetThreadResponse(settings.BaseUrl, notification.RequestedBy);
            await twitterRepository.ReplyToTweetAsync(notification.IdOfTweetBeingRepliedTo.Value, response);
            logger.LogInformation($"Send back this response ->{response}");
        }
    }
}
