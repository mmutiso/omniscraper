using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading;

namespace Omniscraper.Daemon
{
    public class TweetListeningBackgroundService : BackgroundService
    {
        ILogger<TweetListeningBackgroundService> logger;
        public TweetListeningBackgroundService(ILogger<TweetListeningBackgroundService> logger)
        {
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.FromResult(1);
                logger.LogInformation("Running at this time {}", DateTime.UtcNow);

                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
