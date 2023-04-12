using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.Core.TwitterScraper
{
    public interface ITwitterRepository
    {
       // public Task<RawTweet> FindByIdAsync(long id);
        public Task ReplyToTweetAsync(string idOftweetToReplyTo, string content);
     }
}
