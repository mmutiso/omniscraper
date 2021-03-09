using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class RawTweetv2
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string InReplyToStatusId { get; set; }
        public string AuthorScreenName { get; set; }
    }
}
