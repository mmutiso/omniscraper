using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public record TwitterConversation
    {
        public long ConversationId { get; init; }
        public string AuthorDisplayName { get; init; }
        public string AuthorTwitterUsername { get; init; }
        public string AuthorProfilePictureLink { get; init; }
        public List<TweetContent> Tweets { get; init; }
    }

    public record TweetContent
    {
        public long Id { get; init; }
        public string Text { get; init; }
    }
}
