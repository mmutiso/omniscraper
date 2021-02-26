using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class SelfTweetHandler : AbstractTweetContentHandler
    {

        public override async Task HandleAsync<T>(ContentRequestNotification notification, ILogger<T> logger)
        {
            await Task.CompletedTask;

            if (notification.RequestedBy.Equals("omniscraper", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("This is my tweet. Ignoring");
                return;
            }
            {
                await base.HandleAsync(notification, logger);
            }
        }
    }
}
