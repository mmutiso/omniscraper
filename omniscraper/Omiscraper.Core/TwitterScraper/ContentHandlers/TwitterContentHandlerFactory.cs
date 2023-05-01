using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Omniscraper.Core.Infrastructure;
using Omniscraper.Core.Storage;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class TwitterContentHandlerFactory
    {
        ILogger<TwitterContentHandlerFactory> logger;
        ITwitterRepository twitterRepository;
        IScraperRepository scraperDatabaseRepository;
        IOptions<TweetProcessorSettings> options;
        OpenAICompleter openaiCompleter;

        public TwitterContentHandlerFactory(ITwitterRepository twitterRepository, IScraperRepository scraperRepository, 
            ILogger<TwitterContentHandlerFactory> logger, IOptions<TweetProcessorSettings> options, OpenAICompleter openaiCompleter)
        {
            this.logger = logger;
            this.twitterRepository = twitterRepository;
            scraperDatabaseRepository = scraperRepository;
            this.options = options;
            this.openaiCompleter = openaiCompleter;
        }

        public ITweetContentHandler BuildHandlerPipeline()
        {
            ITweetContentHandler selfTweetHandler = new SelfTweetHandler(options);
            ITweetContentHandler notAReplyHandler = new NotAReplyHandler();
            ITweetContentHandler seenVideoHandler = new SeenVideoHandler(scraperDatabaseRepository, options, twitterRepository, openaiCompleter);
            ITweetContentHandler videoHandler = new TweetVideoHandler(scraperDatabaseRepository, twitterRepository, options, openaiCompleter);

            selfTweetHandler
                .SetNext(notAReplyHandler)
                .SetNext(seenVideoHandler)
                .SetNext(videoHandler);

            return selfTweetHandler;
        }
    }
}
