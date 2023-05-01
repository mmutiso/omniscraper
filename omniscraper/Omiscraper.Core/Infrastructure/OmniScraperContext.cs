using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.OAuth;
using Microsoft.Extensions.Logging;

namespace Omniscraper.Core.Infrastructure
{
    public class OmniScraperContext
    {
        TwitterContext _appOnlycontext;
        TwitterContext _userImpersonatingcontext;
        ILogger<OmniScraperContext> _logger;

        public OmniScraperContext(TwitterKeys keys, ILogger<OmniScraperContext> logger)
        {
            _appOnlycontext = new TwitterContext(ApplicationOnlyAuthorizer(keys));
            _userImpersonatingcontext = new TwitterContext(UserImpersonatingAuthorizer(keys));
            _logger = logger;
        }

        public  IQueryable<Streaming> CreateStream(CancellationToken cancellationToken)
        {

            var streamDefinition = from strm in _appOnlycontext.Streaming
                                        .WithCancellation(cancellationToken)
                                   where strm.Type == StreamingType.Filter
                                 && strm.TweetFields == "referenced_tweets,author_id"
                                 && strm.UserFields == "name,username"
                                   && strm.Expansions == "author_id" //expanding on the author Id includes the basic user entity which has the username
                                   select strm;

            return streamDefinition;
        }

        //public async Task<Status> GetTweetByIdAsync(long tweetId)
        //{
        //    var tweetQuery = from tweet in context.Status
        //                     .Where(tweet => tweet.Type == StatusType.Show)
        //                     .Where(tweet => tweet.ID == (ulong)tweetId)
        //                     .Where(tweet => tweet.TweetMode == TweetMode.Extended)
        //                     select tweet;

        //    return await tweetQuery.FirstOrDefaultAsync();
        //}

        public async Task ReplyToTweetAsync(string parentTweetId, string content)
        {
            await _userImpersonatingcontext.ReplyAsync(content, parentTweetId);
        }

        IAuthorizer ApplicationOnlyAuthorizer(TwitterKeys keys)
        {
            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = keys.ConsumerKey,
                    ConsumerSecret = keys.ConsumerSecret
                },
            };

            auth.AuthorizeAsync().GetAwaiter().GetResult(); // find a better way to do this.

            return auth;
        }

        IAuthorizer UserImpersonatingAuthorizer(TwitterKeys keys)
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

            auth.AuthorizeAsync().GetAwaiter().GetResult(); // same here, find a better way to do this

            return auth;
        }
    }
}
