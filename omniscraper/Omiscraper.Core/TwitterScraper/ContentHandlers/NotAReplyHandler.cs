using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class NotAReplyHandler: AbstractTweetContentHandler
    {
        public override async Task HandleAsync<T>(ContentRequestNotification notification, ILogger<T> logger)
        {
            await Task.CompletedTask;

            if (!notification.IdOfTweetBeingRepliedTo.HasValue) // this tweet is not in response to any tweet
            {
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning("Received tweet with id {} that is not a reply to another", notification.IdOfRequestingTweet);
                }
                return;
            }
            else
            {
                await base.HandleAsync(notification, logger);
            }
        }
    }
}
