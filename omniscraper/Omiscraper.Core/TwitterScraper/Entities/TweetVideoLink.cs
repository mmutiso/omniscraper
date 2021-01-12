using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public record TweetVideoLink
    {
        public int BitRate { get; init; }
        public string Url { get; init; }        
    }
}
