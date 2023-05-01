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

        /// <summary>
        /// This is an extension method in C# that extends the TwitterVideo class with a method named GetResponseContent(). The method takes three parameters:
        /// baseUrl: a string representing the base URL for the video.
        /// requestor: a string representing the username of the Twitter user who requested the video.
        /// completionResponse: a string representing the response message to the user after the video has been processed.
        /// </summary>
        /// <param name="video"></param>
        /// <param name="baseUrl"></param>
        /// <param name="requestor"></param>
        /// <param name="completionResponse"></param>
        /// <returns></returns>
        public static string GetResponseContent(this TwitterVideo video, string baseUrl, string requestor, string completionResponse)
        {
            string linkUrl = $"{baseUrl}/{video.Slug}";
            string content = string.Empty;

            if (string.IsNullOrWhiteSpace(completionResponse))
            {
                content = $"Hey @{requestor}, here is the video you requested {linkUrl}";
                return content;
            }

            content = $"{completionResponse}\n{linkUrl}";
            return content;
        }

    }
}
