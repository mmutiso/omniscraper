using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter.OAuth;
using Microsoft.Extensions.Logging;
using LinqToTwitter;

namespace Omniscraper.Api.Models
{
    public class ThreadsService
    {

        ILogger<ThreadsService> _logger;
        TwitterContext _twitterContext;
        public ThreadsService(ILogger<ThreadsService> logger, TwitterContext twitterContext)
        {
            _logger = logger;
            _twitterContext = twitterContext;
        }

        public async Task<List<Tweet>> GetAuthorsTweetsInConversationAsync(string conversationId, string authorId)
        {
            Task<Tweet> originalTweetTask = SingleTweetLookUpAsync(conversationId);

            Task<List<Tweet>> tweetsTask = GetAllTweetsInThreadAsync(conversationId, authorId);

            await Task.WhenAll(originalTweetTask, tweetsTask);

            List<Tweet> results = new List<Tweet>();
            if (originalTweetTask.Result != null)
                results.Add(originalTweetTask.Result);
            if (tweetsTask.Result != null)
                results.AddRange(tweetsTask.Result);

            return results;
        }

        async Task<Tweet> SingleTweetLookUpAsync(string tweetId)
        {
            var result = await
                (from t in _twitterContext.Tweets
                 where t.Ids == tweetId &&
                 t.TweetFields == "conversation_id,author_id" &&
                 t.Type == TweetType.Lookup
                 select t)
                 .SingleOrDefaultAsync();

            if (result.Tweets == null)
                return null;
            else
                return result.Tweets[0];
        }

        /// <summary>
        /// Follow this documentation to help with query construction
        /// https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query
        /// </summary>
        /// <param name="twitterCtx"></param>
        /// <returns></returns>
        async Task<List<Tweet>> GetAllTweetsInThreadAsync(string conversationId, string authorId)
        {
            string searchTerm = $"conversation_id:{conversationId} (from:{authorId})";

            TwitterSearch? searchResponse = await
                (from search in _twitterContext.TwitterSearch
                 where search.Type == SearchType.RecentSearch &&
                       search.Query == searchTerm &&
                       search.MaxResults == 100 &&
                       search.TweetFields == "attachments,conversation_id,in_reply_to_user_id,author_id,referenced_tweets" &&
                       search.Expansions == "author_id,referenced_tweets.id,referenced_tweets.id.author_id"

                 select search)
                .SingleOrDefaultAsync();

            if (searchResponse.Tweets == null)
                return null;

            List<Tweet> tweets =  ParseThread(searchResponse.Tweets, authorId, conversationId);
            return tweets;
        }

        /// <summary>
        /// 2 or more tweets qualify to be a thread
        /// https://help.twitter.com/en/using-twitter/create-a-thread
        /// 
        /// This will not have the first tweet that started the conversation, seems like we might have to get from elsewhere.
        /// </summary>
        /// <param name="tweets"></param>
        /// <param name="authorId"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        List<Tweet> ParseThread(List<Tweet> tweets, string authorId, string conversationId)
        {

            var thread = new List<Tweet>();
            string currentTweetId = conversationId;

            foreach (var tweet in tweets.OrderBy(c => c.ID))
            {
                if (tweet.AuthorID == authorId && tweet.ReferencedTweets[0].ID == currentTweetId && tweet.ReferencedTweets[0].Type == "replied_to")
                {
                    thread.Add(tweet);
                    currentTweetId = tweet.ID;
                }
            }

            return thread;
        }
    }
}
