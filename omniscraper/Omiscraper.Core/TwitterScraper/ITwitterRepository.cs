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
        public Task<RawTweetv2> FindByIdAsync(string id);
        public Task ReplyToTweetAsync(long idOftweetToReplyTo, string content);
     }
}
