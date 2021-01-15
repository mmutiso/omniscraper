using Omniscraper.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.Core.TwitterScraper
{
    public class LinqToTwitterRepository : ITwitterRepository
    {
        OmniScraperContext context;
        public LinqToTwitterRepository(OmniScraperContext scraperContext)
        {
            context = scraperContext;
        }

        public async Task<TweetNotification> FindByIdAsync(long id)
        {
            var tweet = await context.GetStatusByIdAsync(id);
            string tweetStr = JsonConvert.SerializeObject(tweet);
            Console.WriteLine("-----------------------------");
            Console.WriteLine(tweetStr);
            Console.WriteLine("-----------------------------");
            var rawTweet = JsonConvert.DeserializeObject<RawTweet>(tweetStr);
            var notification = new TweetNotification(rawTweet);
            return notification;
        }
    }
}
