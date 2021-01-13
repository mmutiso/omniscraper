using System;
using Omniscraper.Core.Infrastructure;
using System.Linq;
using LinqToTwitter;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Omniscraper.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILoadApplicationCredentials credentialsLoader = new EnvironmentVariablesKeysLoader();
            TwitterKeys keys = credentialsLoader.Load();

            OmniScraperContext context = new OmniScraperContext(keys);

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
                     Status tweet = JsonConvert.DeserializeObject<Status>(content.Content);
                     Console.WriteLine(tweet.Text);
                 }

             })); // register the stream handler
            Console.WriteLine("Passed stream handler registration");
            await task;// start the stream                         
        } 
    }
}
