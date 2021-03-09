using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public class TwitterStreamModelFactory
    {

        public TwitterStreamModel GetTwitterStreamModel(RawTweetv2 rawTweet)
        {
            string authorId = rawTweet.Data.AuthorId;
            string screenName = rawTweet.Includes.Users
                                    .Where(x => x.Id == authorId)
                                    .Select(x=>x.Username)
                                    .FirstOrDefault();

            string IdOfTweetBeingRepliedTo = string.Empty;

            if (rawTweet.Data?.ReferencedTweets != null)
            {
                IdOfTweetBeingRepliedTo = rawTweet.Data.ReferencedTweets
                                                .Where(x => x.Type == "replied_to")
                                                .Select(x => x.Id)
                                                .FirstOrDefault();
            }            

            var model = new TwitterStreamModel
            {
                ConversationId = rawTweet.Data.ConversationId,
                TweetId = long.Parse(rawTweet.Data.Id),
                RequesterUserName = screenName,
                IdIfTweetBeingRepliedTo = string.IsNullOrEmpty(IdOfTweetBeingRepliedTo) ? null : long.Parse(IdOfTweetBeingRepliedTo)
            };

            return model;
        }
    }
}
