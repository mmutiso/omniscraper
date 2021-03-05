using LinqToTwitter;
using LinqToTwitter.OAuth;
using Omniscraper.Core.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.IO;

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
            await AddRulesAsync(context);
            //await ValidateRulesAsync(context);
            await DoFilterStreamAsync(context);


            Console.WriteLine("Assigned rules");
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
                     && strm.TweetFields == "conversation_id"
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
                    Console.WriteLine($"\nTweet ID: {tweet.ID}, Tweet Text: {tweet.Text}, Conversation ID: {tweet.ConversationID}");
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
