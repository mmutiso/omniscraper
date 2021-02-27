using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Omniscraper.Core.Infrastructure;
using Omniscraper.Core;
using LinqToTwitter;

namespace Omniscraper.Daemon
{
    public class TweetListeningBackgroundService : BackgroundService
    {
        ILogger<TweetListeningBackgroundService> logger;
        OmniScraperContext omniContext;
        TweetProcessingService tweetProcessingService;
        TweetProcessorSettings settings;

        public TweetListeningBackgroundService(ILogger<TweetListeningBackgroundService> logger, OmniScraperContext omniScraperContext,
             TweetProcessingService tweetProcessingService, IOptions<TweetProcessorSettings> options)
        {
            this.logger = logger;
            omniContext = omniScraperContext;
            this.tweetProcessingService = tweetProcessingService;
            settings = options.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> keywords = new List<string>
                    {
                        //"omniscraper"
                        settings.StreamListeningKeyword
                    };

                IQueryable<Streaming> twitterStream = omniContext.CreateStream(keywords, stoppingToken);
                logger.LogInformation("Passed stream creation");
                Task streamTask = twitterStream.StartAsync((content) => Task.Factory.StartNew(async () =>
                {
                    if (content.Content.Length > 2)
                    {
                        try
                        {
                            await tweetProcessingService.ProcessTweetAsync(content.Content);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred when processing tweet");
                        }
                    }
                    else
                    {
                        logger.LogInformation("Received keep-alive message");
                    }

                })); // register the stream handler
                logger.LogInformation("Passed stream handler registration");

                Task[] tasks = new[] { streamTask };

                await Task.WhenAny(tasks);
            }
        }
    }
}
