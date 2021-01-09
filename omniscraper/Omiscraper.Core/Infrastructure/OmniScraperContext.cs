using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqToTwitter;
using LinqToTwitter.OAuth;

namespace Omniscraper.Core.Infrastructure
{
    public class OmniScraperContext
    {
        TwitterContext context;

        public OmniScraperContext(TwitterKeys keys)
        {
            context = new TwitterContext(ApplicationAuthorizer(keys));
        }

        private async Task SetKeywordsToTrack(List<string> keywordsToTrack)
        {
            var rules = new List<StreamingAddRule>();

            keywordsToTrack.ForEach(keyword =>
            {
                StreamingAddRule rule = new StreamingAddRule { Tag = "mention", Value = keyword };
                rules.Add(rule);
            });
            
            Streaming? result = await context.AddStreamingFilterRulesAsync(rules);
            StreamingMeta? meta = result?.Meta;
            if (meta?.Summary != null)
            {
                Console.WriteLine($"\nSent: {meta.Sent}");

                StreamingMetaSummary summary = meta.Summary;

                Console.WriteLine($"Created:  {summary.Created}");
                Console.WriteLine($"!Created: {summary.NotCreated}");
            }
        }

        public async Task<IQueryable<Streaming>> CreateStream(List<string> keywordsToTrack, CancellationToken cancellationToken)
        {
            await SetKeywordsToTrack(keywordsToTrack);

            var stream = from strm in context.Streaming
                         .WithCancellation(cancellationToken)
                         where strm.Type == StreamingType.Filter  
                         select strm;

            return stream;
        }
        
        IAuthorizer ApplicationAuthorizer(TwitterKeys keys)
        {
            var auth = new SingleUserAuthorizer()
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = keys.AccessToken,
                    ConsumerSecret = keys.ConsumerSecret,
                    AccessToken = keys.AccessToken,
                    AccessTokenSecret = keys.AccessTokenSecret
                },
            };
            return auth;
        }


    }
}
