using System;
using Omniscraper.Core.Infrastructure;
using System.Linq;
using LinqToTwitter;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Omniscraper.Core.TwitterScraper.Entities;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core;
using System.Diagnostics;
using Omniscraper.Core.Storage;
using LinqToTwitter.OAuth;

namespace Omniscraper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await FetchTweet("1366017614800117762");

            //await AuthSampleAsync();
        }


        static async Task<IAuthorizer> AuthSampleAsync()
        {
            var keys = await GetKeysAsync();

            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = keys.ConsumerKey,
                    ConsumerSecret = keys.ConsumerSecret,
                    AccessToken = "",
                    AccessTokenSecret = ""
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

        static async Task TweetSampleAsync()
        {
            var ctx = new TwitterContext(await AuthSampleAsync());
            await ctx.TweetAsync("Just another day, testing");
        }

        static async Task FetchTweet(string id)
        {
            var ctx = new OmniScraperContext(await GetKeysAsync(), default);
            ITwitterRepository twitterRepository = new LinqToTwitterRepository(ctx);
            RawTweetv2 videoTweet = await twitterRepository.FindByIdAsync(id);
            TweetNotification tweetNotification = new TweetNotification(videoTweet, default, default);

            if (tweetNotification.HasVideo())
            {
                var video = tweetNotification.GetVideo();


                Console.WriteLine("Done checking for videos");
            }

            static async Task StreamMain()
            {
                ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader(default);
                TwitterKeys keys = credentialsLoader.Load();

                OmniScraperContext context = new OmniScraperContext(keys, default);
                ITwitterRepository twitterRepository = new LinqToTwitterRepository(context);

                List<string> keywords = new List<string>
            {
                "omniscraper"
            };
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                IQueryable<Streaming> twitterStream = await context.CreateStreamAsync(keywords, tokenSource.Token);

                Console.WriteLine("Passed stream creation");

                Task task = twitterStream.StartAsync((content) => Task.Factory.StartNew(() =>
                {
                    Console.WriteLine(content.Content);

                    if (content.Content.Length > 2)
                    {
                        Console.WriteLine();

                        Console.WriteLine(content.Content);
                    }
                    else
                    {
                        Console.WriteLine("Received keep-alive message");
                    }

                })); // register the stream handler
                Console.WriteLine("Passed stream handler registration");
                await task;// start the stream    
            }
        }
    }
}
