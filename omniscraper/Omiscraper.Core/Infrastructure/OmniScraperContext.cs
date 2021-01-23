using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;

namespace Omniscraper.Core.Infrastructure
{
    public class OmniScraperContext
    {
        TwitterContext context;

        public OmniScraperContext(TwitterKeys keys)
        {
            context = new TwitterContext(ApplicationAuthorizer(keys));
        }

        public IQueryable<Streaming> CreateStream(List<string> keywordsToTrack, CancellationToken cancellationToken)
        {
            string keywords = string.Join(",", keywordsToTrack);

            var stream = from strm in context.Streaming
                         .WithCancellation(cancellationToken)
                         where strm.Type == StreamingType.Filter &&
                         strm.Track == keywords
                         select strm;

            return stream;
        }

        public async Task<Status> GetTweetByIdAsync(long tweetId)
        {
            var tweetQuery = from tweet in context.Status
                             .Where(tweet => tweet.Type == StatusType.Show)
                             .Where(tweet => tweet.ID == (ulong)tweetId)
                             .Where(tweet => tweet.TweetMode == TweetMode.Extended)
                             select tweet;

            return await tweetQuery.FirstOrDefaultAsync();
        }

        public async Task ReplyToTweetAsync(long parentTweetId, string content)
        {
            await context.ReplyAsync((ulong)parentTweetId, content);
        }

        IAuthorizer ApplicationAuthorizer(TwitterKeys keys)
        {
            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = keys.ConsumerKey,
                    ConsumerSecret = keys.ConsumerSecret,
                    AccessToken = keys.AccessToken,
                    AccessTokenSecret = keys.AccessTokenSecret
                },
            };
            return auth;
        }


    }
}
