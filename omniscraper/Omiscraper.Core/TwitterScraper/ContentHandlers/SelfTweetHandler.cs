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
    public class SelfTweetHandler : AbstractTweetContentHandler
    {

        TweetProcessorSettings settings;
        public SelfTweetHandler(IOptions<TweetProcessorSettings> options)
        {
            settings = options.Value;
        }

        public override async Task HandleAsync<T>(StreamedTweetContent notification, ILogger<T> logger)
        {
            await Task.CompletedTask;

            //if (notification.RequestedBy.Equals(settings.StreamListeningKeywords, StringComparison.OrdinalIgnoreCase))
            //{
            //    logger.LogInformation("This is my tweet. Ignoring");
            //    return;
            //}
            {
                await base.HandleAsync(notification, logger);
            }
        }
    }
}
