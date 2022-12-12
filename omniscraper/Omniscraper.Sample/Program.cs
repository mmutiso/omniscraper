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

namespace Omniscraper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await FetchTweet(1366017614800117762);

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

            ILoadApplicationKeys credentialsLoader = new EnvironmentVariablesKeysLoader(default);
            TwitterKeys keys = credentialsLoader.LoadTwitterKeys();
            return keys;
        }

        static async Task TweetSampleAsync()
        {
            var ctx = new TwitterContext(await AuthSampleAsync());
            await ctx.TweetAsync("Just another day, testing");
        }

        static async Task FetchTweet(long id)
        {
            var ctx = new OmniScraperContext(await GetKeysAsync());
            ITwitterRepository twitterRepository = new LinqToTwitterRepository(ctx);
            RawTweet videoTweet = await twitterRepository.FindByIdAsync(id);
            TweetNotification tweetNotification = new TweetNotification(videoTweet, default, default);

            if (tweetNotification.HasVideo())
            {
                var video = tweetNotification.GetVideo();


                Console.WriteLine("Done checking for videos");
            }

            static async Task StreamMain()
            {
                ILoadApplicationKeys credentialsLoader = new EnvironmentVariablesKeysLoader(default);
                TwitterKeys keys = credentialsLoader.LoadTwitterKeys();

                OmniScraperContext context = new OmniScraperContext(keys);
                ITwitterRepository twitterRepository = new LinqToTwitterRepository(context);

                List<string> keywords = new List<string>
            {
                "omniscraper"
            };
                CancellationTokenSource tokenSource = new CancellationTokenSource();

                IQueryable<Streaming> twitterStream = context.CreateStream(keywords, tokenSource.Token);

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
