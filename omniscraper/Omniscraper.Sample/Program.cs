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

namespace Omniscraper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StreamMain();

            //await AuthSampleAsync();
        }

        static async Task AuthSampleAsync()
        {
            ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader(default);
            TwitterKeys keys = credentialsLoader.Load();

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

            var ctx = new TwitterContext(auth);
            await ctx.TweetAsync("Just another day, testing");
        }

        static async Task StreamMain()
        {
            ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader(default);
            TwitterKeys keys = credentialsLoader.Load();

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
