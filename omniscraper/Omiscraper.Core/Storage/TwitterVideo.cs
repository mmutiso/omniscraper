using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class TwitterVideo: BaseTwitterContent
    {
        public string Url { get; set; }

        public TwitterVideo()
        {

        }

        public TwitterVideo(Guid id, string url, long tweetId, long parentTweetId)
            :base(id, DateTime.UtcNow, tweetId, parentTweetId)
        {
            Url = url;
        }
    }
}
