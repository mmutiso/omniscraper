using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.Core.TwitterScraper
{
    public static class TweetExtensions
    {
        public static List<TweetVideoLink> GetVideoLinks(this List<Variant> variants)
        {
            var links = new List<TweetVideoLink>(variants.Count);
            variants.ForEach(variant =>
            {
                var link = new TweetVideoLink
                {
                    BitRate = variant.BitRate,
                    Url = variant.Url
                };
                links.Add(link);
            });
            return links;
        }

        public static string GetResponseContent(this TwitterVideo video, string baseUrl, string requestor)
        {
            string linkUrl = $"{baseUrl}/{video.Slug}";
            string content = $"@{requestor} Here you go {linkUrl}";
            return content;
        }

    }
}
