using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class ContentRequestNotification
    {        
        public long? IdOfTweetBeingRepliedTo { get; set; }
        public long IdOfRequestingTweet { get; set; }
        public string RequestedBy { get; set; }


    }
}
