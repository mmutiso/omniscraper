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
        TwitterContext context;
        ILogger<OmniScraperContext> logger;

        public OmniScraperContext(TwitterKeys keys, ILogger<OmniScraperContext> logger)
        {
            context = new TwitterContext(ApplicationAuthorizer(keys));
            this.logger = logger;
        }

        async Task RegisterStreamingKeywords(List<string> keywords)
        {
            var streamingRules = new List<StreamingAddRule>(keywords.Count);
            keywords.ForEach(keyword =>
            {
                streamingRules.Add(new StreamingAddRule { Tag = keyword, Value = keyword });
            });

            Streaming? result = await context.AddStreamingFilterRulesAsync(streamingRules);
            StreamingMeta? meta = result?.Meta;
            if (meta?.Summary != null)
            {
                logger.LogInformation($"Sent rules: {meta.Sent}");

                StreamingMetaSummary summary = meta.Summary;
                logger.LogInformation($"Created:  {summary.Created}");
                logger.LogInformation($"!Created: {summary.NotCreated}");
            }

            if (result?.Errors != null && result.HasErrors)
                result.Errors.ForEach(error =>
                    logger.LogWarning(
                        $"\nTitle: {error.Title}" +
                        $"\nValue: {error.Value}" +
                        $"\nID:    {error.ID}" +
                        $"\nType:  {error.Type}"));
        }

        public async Task<IQueryable<Streaming>> CreateStreamAsync(List<string> keywordsToTrack, CancellationToken cancellationToken)
        {
            await RegisterStreamingKeywords(keywordsToTrack);

            var stream = from strm in context.Streaming
                         .WithCancellation(cancellationToken)
                         where strm.Type == StreamingType.Filter
                         && strm.TweetFields == "conversation_id,in_reply_to_user_id,author_id"
                         && strm.Expansions == "author_id" //expanding on the author Id includes the basic user entity which has the username
                         select strm;

            return stream;
        }

        public async Task<Tweet> GetTweetByIdAsync(string tweetId)
        {
            var tweetQuery = from tweet in context.Tweets
                             .Where(tweet => tweet.Ids == tweetId)
                             .Where(x=>x.TweetFields == "conversation_id")
                             .Where(tweet => tweet.Type == TweetType.Lookup)
                             select tweet;
            var result = await tweetQuery.FirstOrDefaultAsync();

            if (result?.Tweets != null)
            {
                if (result.Tweets.Count > 0)
                {
                    return result.Tweets[0];
                }
            }
            logger.LogWarning($"Tweet with Id {tweetId} not found");
            return default;
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
