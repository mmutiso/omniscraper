using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class ScraperRepository : IScraperRepository
    {
        IDbContextFactory<OmniscraperDbContext> contextFactory;
        public ScraperRepository(IDbContextFactory<OmniscraperDbContext> dbContextFactory)
        {
            contextFactory = dbContextFactory;
        }

        public  bool GetIfVideoExists(long tweetId, out TwitterVideo video)
        {
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
                return exists;
            }           
        }

        public async Task<TwitterVideo> GetTwitterVideoAsync(Guid id)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                var video = await context.FindAsync<TwitterVideo>(id);

                return video;
            }
        }

        public async Task CaptureTwitterVideoAndRequestAsync(TwitterVideoRequest request, TwitterVideo twitterVideo)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                context.Add(twitterVideo);
                context.Add(request);
                await SaveAsync(context);
            }
        }

        public async Task CaptureTwitterRequestAsync(TwitterVideoRequest request)
        {
            using (var context = contextFactory.CreateDbContext())
            {
                context.Add(request);
                await SaveAsync(context);
            }
        }

        async Task SaveAsync(OmniscraperDbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
