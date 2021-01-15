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

namespace Omniscraper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader();
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
                     RawTweet tweet = JsonConvert.DeserializeObject<RawTweet>(content.Content);
                     long? replyingToId = tweet.in_reply_to_status_id;
                     if (replyingToId.HasValue)
                     {
                         TweetNotification replyingTo = twitterRepository
                                                        .FindByIdAsync(replyingToId.Value)
                                                        .GetAwaiter()
                                                        .GetResult();

                         if (replyingTo.HasVideo())
                         {
                             var videoLinks = replyingTo.GetVideoLinks();
                             videoLinks.ForEach(vl =>
                             {
                                 Console.WriteLine(vl.Url);
                             });
                         }
                         else
                         {
                             Console.WriteLine("No video here");
                         }
                     }  
                 }

             })); // register the stream handler
            Console.WriteLine("Passed stream handler registration");
            await task;// start the stream                         
        } 
    }
}
