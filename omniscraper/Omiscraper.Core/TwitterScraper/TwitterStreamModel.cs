using Omniscraper.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public class TwitterStreamModel
    {
        public long TweetId { get; set; }
        public long? IdIfTweetBeingRepliedTo { get; set; }
        public string RequesterUserName { get; set; }
        public string ConversationId { get; set; }

        internal bool HasVideo()
        {
            throw new NotImplementedException();
        }

        internal TwitterVideo GetVideo()
        {
            throw new NotImplementedException();
        }
    }
}
