using Omniscraper.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.TwitterScraper.Entities;
using System.Text.Json;

namespace Omniscraper.Core.TwitterScraper
{
    public class LinqToTwitterRepository : ITwitterRepository
    {
        OmniScraperContext context;
        public LinqToTwitterRepository(OmniScraperContext scraperContext)
        {
            context = scraperContext;
        }

        public async Task<RawTweetv2> FindByIdAsync(string id)
        {
            var tweet = await context.GetTweetByIdAsync(id);
            string tweetStr = JsonSerializer.Serialize(tweet);
            var rawTweet = JsonSerializer.Deserialize<RawTweetv2>(tweetStr);

            return rawTweet;
        }

        public async Task ReplyToTweetAsync(long idOftweetToReplyTo, string content)
        {
            await context.ReplyToTweetAsync(idOftweetToReplyTo, content);

            
        }
    }
}
