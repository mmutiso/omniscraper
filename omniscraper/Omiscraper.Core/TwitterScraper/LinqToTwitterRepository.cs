using Omniscraper.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //public async Task<RawTweet> FindByIdAsync(long id)
        //{
        //    var tweet = await context.GetTweetByIdAsync(id);
        //    string tweetStr = JsonConvert.SerializeObject(tweet);
        //    var rawTweet = JsonConvert.DeserializeObject<RawTweet>(tweetStr);
        //    return rawTweet;
        //}

        public async Task ReplyToTweetAsync(string idOftweetToReplyTo, string content)
        {
            await context.ReplyToTweetAsync(idOftweetToReplyTo, content);

            
        }
    }
}
