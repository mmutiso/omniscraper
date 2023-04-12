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
using Microsoft.EntityFrameworkCore;
using Omniscraper.Core.Storage;

namespace Omniscraper.Daemon
{
    public class TweetListeningBackgroundService : BackgroundService
    {
        ILogger<TweetListeningBackgroundService> logger;
        OmniScraperContext omniContext;
        TweetProcessingService tweetProcessingService;
        TweetProcessorSettings settings;
        IDbContextFactory<OmniscraperDbContext> dbFactory;

        public TweetListeningBackgroundService(ILogger<TweetListeningBackgroundService> logger, OmniScraperContext omniScraperContext,
             TweetProcessingService tweetProcessingService, IOptions<TweetProcessorSettings> options,
             IDbContextFactory<OmniscraperDbContext> factory)
        {
            this.logger = logger;
            omniContext = omniScraperContext;
            this.tweetProcessingService = tweetProcessingService;
            settings = options.Value;
            dbFactory = factory;
        }
       
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Passed service start");
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Cancellation token requested: {stoppingToken.IsCancellationRequested}");
               

                IQueryable<Streaming> twitterStream = omniContext.CreateStream(stoppingToken);
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
                            logger.LogError("An exception was thrown when processing tweet. See the error message and tweet below");
                            logger.LogError(ex.Message);
                            logger.LogInformation(content.Content);
                        }
                    }
                    else
                    {
                        logger.LogInformation("Received keep-alive message");
                    }

                })); // register the stream handler
                logger.LogInformation("The stream exited. Pausing for 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10));

                Task[] tasks = new[] { streamTask };

                await Task.WhenAny(tasks);
                var exception = (Exception)tasks[0].Exception.InnerException;
            }
        }
    }
}
