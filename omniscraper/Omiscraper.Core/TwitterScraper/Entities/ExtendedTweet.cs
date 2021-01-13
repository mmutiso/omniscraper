using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class ExtendedTweet
    {
        public string full_text { get; set; }
        public int[] display_text_range { get; set; }
        public TweetEntities entities { get; set; }
        public ExtendedTweetEntities extended_entities { get; set; }
    }
}
