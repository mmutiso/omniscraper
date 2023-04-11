using Omniscraper.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities.v2
{
    public class StreamedTweetContent
    {
        public Data data { get; set; }
        public Includes includes { get; set; }
        public List<MatchingRule> matching_rules { get; set; }

        public TwitterVideoRequest GenerateVideoRequest()
        {
            Guid videoId = Guid.NewGuid();
            var request = new TwitterVideoRequest(long.Parse(data.id), videoId, GetAuthorUsername());

            return request;
        }

        public string GetTweetText()
        {
            return data.text;
        }

        public long? TweetRepliedToId => GetTweetBeingRepliedTo();
        public long RequestingTweetId => long.Parse(data.id);

        long? GetTweetBeingRepliedTo() 
        {
            var tweet = data.referenced_tweets.Where(x => x.type == "replied_to").FirstOrDefault();
            long tweetId ;
            if (tweet is null)
                return null;

            bool valid = long.TryParse(tweet.id, out tweetId);

            if (valid)
                return tweetId;
            else
                return null;
        }

        public string RequestedBy => GetAuthorUsername();

        string GetAuthorUsername()
        {
            string username = includes.users.Where(x=>x.id == data.author_id).FirstOrDefault().username;

            return username;
        }
    }

    public class Data
    {
        public string author_id { get; set; }
        public List<string> edit_history_tweet_ids { get; set; }
        public string id { get; set; }
        public List<ReferencedTweet> referenced_tweets { get; set; }
        public string text { get; set; }
    }

    public class Includes
    {
        public List<User> users { get; set; }
    }

    public class MatchingRule
    {
        public string id { get; set; }
        public string tag { get; set; }
    }

    public class ReferencedTweet
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
    }


}
