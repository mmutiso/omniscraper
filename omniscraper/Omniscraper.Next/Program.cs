using LinqToTwitter;
using LinqToTwitter.OAuth;
using Omniscraper.Core.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;
using LinqToTwitter.Common;

namespace Omniscraper.Next
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var authorizer = await AuthSampleAsync();
            await authorizer.AuthorizeAsync();
            var context = new TwitterContext(authorizer);

            //await DeleteRulesAsync(context);
            // await AddRulesAsync(context);
            //await ValidateRulesAsync(context);
            //await DoFilterStreamAsync(context);
            //await SingleTweetLookUpAsync(context);
            await DoSearchAsync(context);

            Console.WriteLine("DONE TESTING");
        }

        static async Task AddRulesAsync(TwitterContext twitterCtx)
        {
            var rules = new List<StreamingAddRule>
            {
                new StreamingAddRule { Tag = "some things", Value = "Katumbiti" },
            };

            Streaming? result = await twitterCtx.AddStreamingFilterRulesAsync(rules);

            Console.WriteLine(result.Ids);
            StreamingMeta? meta = result?.Meta;

            if (meta?.Summary != null)
            {
                Console.WriteLine($"\nSent: {meta.Sent}");

                StreamingMetaSummary summary = meta.Summary;

                Console.WriteLine($"Created:  {summary.Created}");
                Console.WriteLine($"!Created: {summary.NotCreated}");
            }

            if (result?.Errors != null && result.HasErrors)
                result.Errors.ForEach(error =>
                    Console.WriteLine(
                        $"\nTitle: {error.Title}" +
                        $"\nValue: {error.Value}" +
                        $"\nID:    {error.ID}" +
                        $"\nType:  {error.Type}"));
        }

        static async Task SingleTweetLookUpAsync(TwitterContext twitterContext)
        {
            var result = await 
                (from t in twitterContext.Tweets
                 where t.Ids == "1478241311392108545" &&
                 t.TweetFields == "conversation_id,author_id" &&                
                 t.Type == TweetType.Lookup
                 select t)
                 .SingleOrDefaultAsync();

            if (result.Tweets != null)
                result.Tweets.ForEach(tweet =>
                    Console.WriteLine(
                        "\n  User: {0}, Conversation ID {1}, Tweet: {2}",
                        tweet.AuthorID,
                        tweet.ConversationID,
                        tweet.Text));
            else
                Console.WriteLine("No entries found.");
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
        static async Task ParseThread(List<Tweet> tweets, string authorId, string conversationId)
        {
            await Task.CompletedTask;

            var thread = new List<Tweet>();
            string currentTweetId = conversationId;

            foreach (var tweet in tweets.OrderBy(c=>c.ID))
            {
                if (tweet.AuthorID == authorId && tweet.ReferencedTweets[0].ID == currentTweetId && tweet.ReferencedTweets[0].Type == "replied_to")
                {
                    thread.Add(tweet);
                    currentTweetId = tweet.ID;
                }
            }

            thread.ForEach(tweet =>
            {
                Console.WriteLine(tweet.Text);
            });            
        }


        /// <summary>
        /// Follow this documentation to help with query construction
        /// https://developer.twitter.com/en/docs/twitter-api/tweets/search/integrate/build-a-query
        /// </summary>
        /// <param name="twitterCtx"></param>
        /// <returns></returns>

        static async Task DoSearchAsync(TwitterContext twitterCtx)
        {

            //https://twitter.com/LBacaj/status/1478241311392108545
            string searchTerm = "conversation_id:1478241311392108545 (from:38566144)";

            TwitterSearch? searchResponse =
                await
                (from search in twitterCtx.TwitterSearch
                 where search.Type == SearchType.RecentSearch &&
                       search.Query == searchTerm &&
                       search.MaxResults == 100 &&
                       search.TweetFields == "attachments,conversation_id,in_reply_to_user_id,author_id,referenced_tweets" &&
                       search.Expansions == "author_id,referenced_tweets.id,referenced_tweets.id.author_id"

                 select search)
                .SingleOrDefaultAsync();

            Console.WriteLine("-=================================-");
            if (searchResponse?.Tweets != null)
            {
                await ParseThread(searchResponse.Tweets, "38566144", "1478241311392108545");
            }
            else
                Console.WriteLine("No entries found.");
            Console.WriteLine("-=================================-");
        }

        static async Task DeleteRulesAsync(TwitterContext twitterCtx)
        {
            var ruleIds = new List<string>
            {
                "1367859000629411840",
                "1367855205170221057",
                "1367855205170221058"
            };

            Streaming? result = await twitterCtx.DeleteStreamingFilterRulesAsync(ruleIds);

            if (result?.Meta?.Summary != null)
            {
                StreamingMeta meta = result.Meta;
                Console.WriteLine($"\nSent: {meta.Sent}");

                StreamingMetaSummary summary = meta.Summary;

                Console.WriteLine($"Deleted:  {summary.Deleted}");
                Console.WriteLine($"!Deleted: {summary.NotDeleted}");
            }

            if (result?.Errors != null && result.HasErrors)
                result.Errors.ForEach(error =>
                    Console.WriteLine(
                        $"\nTitle: {error.Title}" +
                        $"\nValue: {error.Value}" +
                        $"\nID:    {error.ID}" +
                        $"\nType:  {error.Type}"));
        }

        static async Task DoFilterStreamAsync(TwitterContext twitterCtx)
        {
            Console.WriteLine("\nStreamed Content: \n");
            int count = 0;
            var cancelTokenSrc = new CancellationTokenSource();

            try
            {
                await
                    (from strm in twitterCtx.Streaming
                                            .WithCancellation(cancelTokenSrc.Token)
                     where strm.Type == StreamingType.Filter
                     && strm.TweetFields == "conversation_id,in_reply_to_user_id,author_id"
                     && strm.Expansions== "author_id" //expanding on the author Id includes the basic user entity which has the username
                     select strm)
                    .StartAsync(async strm =>
                    {
                        await HandleStreamResponse(strm);

                        if (count++ >= 5)
                            cancelTokenSrc.Cancel();
                    });
            }
            catch (IOException ex)
            {
                // Twitter might have closed the stream,
                // which they do sometimes. You should
                // restart the stream, but be sure to
                // read Twitter documentation on stream
                // back-off strategies to prevent your
                // app from being blocked.
                Console.WriteLine(ex.ToString());
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }

        static async Task<int> HandleStreamResponse(StreamContent strm)
        {
            if (strm.HasError)
            {
                Console.WriteLine($"Error during streaming: {strm.ErrorMessage}");

            }
            else
            {
                Console.WriteLine(strm.Content);
                Tweet? tweet = strm?.Entity?.Tweet;
                if (tweet != null)
                {
                    Console.WriteLine($"\nTweet ID: {tweet.ID}, Tweet Text: {tweet.Text}, Conversation ID: {tweet.ConversationID}");
                    Console.WriteLine(tweet.InReplyToUserID);
                }
                
                    
            }

            return await Task.FromResult(0);
        }

        static async Task ValidateRulesAsync(TwitterContext twitterCtx)
        {
            var rules = new List<StreamingAddRule>
            {
                new StreamingAddRule { Tag = "memes with media", Value = "meme has:images" },
                new StreamingAddRule { Tag = "cats with media", Value = "cat has:media" }
            };

            Streaming? result = await twitterCtx.AddStreamingFilterRulesAsync(rules, isValidateOnly: true);

            if (result?.Meta?.Summary != null)
            {
                StreamingMeta meta = result.Meta;
                Console.WriteLine($"\nSent: {meta.Sent}");

                StreamingMetaSummary summary = meta.Summary;

                Console.WriteLine($"Created:  {summary.Created}");
                Console.WriteLine($"!Created: {summary.NotCreated}");
            }

            if (result?.Errors != null && result.HasErrors)
                result.Errors.ForEach(error =>
                    Console.WriteLine(
                        $"\nTitle: {error.Title}" +
                        $"\nValue: {error.Value}" +
                        $"\nID:    {error.ID}" +
                        $"\nType:  {error.Type}"));
        }             

        static async Task<IAuthorizer> AuthSampleAsync()
        {
            var keys = await GetKeysAsync();

            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new  InMemoryCredentialStore
                {
                    ConsumerKey = keys.ConsumerKey,
                    ConsumerSecret = keys.ConsumerSecret
                }
            };
            return auth;
        }

        static async Task<TwitterKeys> GetKeysAsync()
        {
            await Task.CompletedTask;

            ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader(default);
            TwitterKeys keys = credentialsLoader.Load();
            return keys;
        }
    }
}
