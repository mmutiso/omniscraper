using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public abstract class AbstractTweetContentHandler : ITweetContentHandler
    {
        private ITweetContentHandler nextHandler;

        public ITweetContentHandler SetNext(ITweetContentHandler handler)
        {
            nextHandler = handler;

            return nextHandler;
        }

        public virtual async Task HandleAsync<T>(TwitterStreamModel twitterStreamModel, ILogger<T> logger)
        {
            if (nextHandler != null)
            {
                await nextHandler.HandleAsync(twitterStreamModel, logger);

                return;
            }
        }       
    }
}
