using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class ScraperRepository : IScraperRepository
    {
        OmniscraperDbContext context;
        public ScraperRepository(OmniscraperDbContext dbContext)
        {
            context = dbContext;
        }

        public  bool GetIfVideoExists(long tweetId, out TwitterVideo video)
        {
            bool exists = context.TwitterVideos
                .Any(x => x.TweetWithVideoId == tweetId);

            video = default;

            if (exists)
            {
                video = context.TwitterVideos
                    .Where(x => x.TweetWithVideoId == tweetId)
                    .Single();

            }
            return exists;
        }

        public async Task<TwitterVideo> GetTwitterVideoAsync(Guid id)
        {
            var video = await context.FindAsync<TwitterVideo>(id);

            return video;
        }

        public async Task SaveTwitterVideoAsync(TwitterVideo twitterVideo)
        {
            context.Add<TwitterVideo>(twitterVideo);
            await context.SaveChangesAsync();
        }
    }
}
