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
       

        public static string GetResponseContent(this TwitterVideo video, string baseUrl)
        {
            string linkUrl = $"{baseUrl}/{video.Slug}";
            string content = $"@{video.RequestedBy} Here you go {linkUrl}";
            return content;
        }       
    }
}
