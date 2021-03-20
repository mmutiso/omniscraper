using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.Core.TwitterScraper
{
    public static class TweetExtensions
    {
        public static List<TweetVideoLink> GetVideoLinks(this List<Variant> variants)
        {
            var links = new List<TweetVideoLink>(variants.Count);
            variants.ForEach(variant =>
            {
                var link = new TweetVideoLink
                {
                    BitRate = variant.BitRate,
                    Url = variant.Url
                };
                links.Add(link);
            });
            return links;
        }

        public static string GetResponseContent(this TwitterVideo video, string baseUrl, string requestor)
        {
            string linkUrl = $"{baseUrl}/{video.Slug}";
            string content = $"@{requestor} Here you go {linkUrl}";
            return content;
        }

        public static string GetThreadResponse(this TwitterThread thread, string baseUrl, string requestor)
        {
            string linkUrl = $"{baseUrl}/threads/{thread.Slug}";
            string content = $"@{requestor} Read the thread here {linkUrl}";

            return content;
        }

        public static ContentRequestNotification CreateRequestNotification(this RawTweet rawTweet)
        {
            var requestNotification = new ContentRequestNotification
            {
                IdOfRequestingTweet = rawTweet.id,
                IdOfTweetBeingRepliedTo = rawTweet.in_reply_to_status_id,
                RequestedBy = rawTweet.user.screen_name
            };

            return requestNotification;
        }

        public static TwitterThread CreateThreadFromConversation(this TwitterConversation conversation)
        {
            Guid id = Guid.NewGuid();
            var thread = new TwitterThread
            {
                Id = id,
                ConversationId = conversation.ConversationId,
                Slug = id.ToString().Split("-")[0],
                AuthorDisplayName = conversation.AuthorDisplayName,
                AuthorProfilePictureLink = conversation.AuthorProfilePictureLink,
                AuthorTwitterUsername = conversation.AuthorTwitterUsername,
                Tweets = new List<Storage.TweetContent>(conversation.Tweets.Count)
            };

            conversation.Tweets.ForEach(tweet =>
            {
                thread.Tweets.Add(new Storage.TweetContent
                {
                    Id = tweet.Id,
                    Text = tweet.Text
                });
            });

            return thread;
        }
    }
}
