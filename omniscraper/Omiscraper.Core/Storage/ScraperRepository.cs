using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.ApplicationInsights;

namespace Omniscraper.Core.Storage
{
    public class ScraperRepository : IScraperRepository
    {
        IDbContextFactory<OmniscraperDbContext> contextFactory;
        TelemetryClient _telemetryClient;
        public ScraperRepository(IDbContextFactory<OmniscraperDbContext> dbContextFactory, TelemetryClient telemetryClient)
        {
            contextFactory = dbContextFactory;
            _telemetryClient = telemetryClient;
        }

        Stopwatch GetStopwatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            return stopwatch;
        }

        void RecordMetric(string metricName, double metricValue)
        {
            var metric = _telemetryClient.GetMetric(metricName);
            metric.TrackValue(metricValue);
        }

        public  bool GetIfVideoExists(long tweetId, out TwitterVideo video)
        {
            var stopWatch = GetStopwatch();
            stopWatch.Start();
            using (var context = contextFactory.CreateDbContext())
            {
                bool exists = context.TwitterVideos
               .Any(x => x.ParentTweetId == tweetId);

                video = default;

                if (exists)
                {
                    video = context.TwitterVideos
                        .Where(x => x.ParentTweetId == tweetId)
                        .FirstOrDefault();
                }
                stopWatch.Stop();
                RecordMetric("videoExistsByTwitterIdInMs", stopWatch.ElapsedMilliseconds);

                return exists;
            } 
        }

        public async Task<TwitterVideo> GetTwitterVideoAsync(Guid id)
        {
            var stopWatch = GetStopwatch();
            stopWatch.Start();
            using (var context = contextFactory.CreateDbContext())
            {
                var video = await context.FindAsync<TwitterVideo>(id);

                stopWatch.Stop();
                RecordMetric("getVideoByIdInMs", stopWatch.ElapsedMilliseconds);
                return video;
            }
        }

        public async Task CaptureTwitterVideoAndRequestAsync(TwitterVideoRequest request, TwitterVideo twitterVideo)
        {
            var stopWatch = GetStopwatch();
            stopWatch.Start();
            using (var context = contextFactory.CreateDbContext())
            {
                context.Add(twitterVideo);
                context.Add(request);
                await SaveAsync(context);
            }
            stopWatch.Stop();
            RecordMetric("saveVideoAndRequestInMs", stopWatch.ElapsedMilliseconds);
        }

        public async Task CaptureTwitterRequestAsync(TwitterVideoRequest request)
        {
            var stopWatch = GetStopwatch();
            stopWatch.Start();
            using (var context = contextFactory.CreateDbContext())
            {
                context.Add(request);
                await SaveAsync(context);
            }
            stopWatch.Stop();
            RecordMetric("saveRequestOnlyInMs", stopWatch.ElapsedMilliseconds);
        }

        async Task SaveAsync(OmniscraperDbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
