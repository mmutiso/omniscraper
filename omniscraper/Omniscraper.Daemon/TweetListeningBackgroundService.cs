using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public TweetListeningBackgroundService(ILogger<TweetListeningBackgroundService> logger, OmniScraperContext omniScraperContext,
             TweetProcessingService tweetProcessingService)
        {
            this.logger = logger;
            omniContext = omniScraperContext;
            this.tweetProcessingService = tweetProcessingService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> keywords = new List<string>
                    {
                        "omniscraper"
                    };

                IQueryable<Streaming> twitterStream = omniContext.CreateStream(keywords, stoppingToken);
                logger.LogInformation("Passed stream creation");
                Task streamTask = twitterStream.StartAsync((content) => Task.Factory.StartNew(() =>
                {
                    if (content.Content.Length > 2)
                    {
                        Console.WriteLine(content.Content);
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
