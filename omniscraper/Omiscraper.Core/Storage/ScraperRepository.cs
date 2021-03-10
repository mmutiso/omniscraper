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

        public async Task<TwitterVideo> GetTwitterVideoAsync(Guid id)
        {
            var video = await context.FindAsync<TwitterVideo>(id);

            return video;
        }

        public async Task CaptureTwitterVideoAndRequestAsync(TwitterVideoRequest request, TwitterVideo twitterVideo)
        {
            context.Add(twitterVideo);
            context.Add(request);
            await context.SaveChangesAsync();
        }

      
    }
}
