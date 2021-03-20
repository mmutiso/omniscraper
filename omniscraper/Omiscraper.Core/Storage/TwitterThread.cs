using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class TwitterThread
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public long ConversationId { get; set; }
        public string AuthorDisplayName { get; set; }
        public string AuthorTwitterUsername { get; set; }
        public string AuthorProfilePictureLink { get; set; }
        public List<TweetContent> Tweets { get; set; }

        public List<TwitterThreadRequest> ThreadRequests { get; set; }
    }

    public record TweetContent
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
