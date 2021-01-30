using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Omniscraper.Core.Storage
{
    public class TwitterVideo: BaseTwitterContent
    {
        public string Url { get; set; }

        public TwitterVideo()
        {

        }

        public TwitterVideo(Guid id, string url, long tweetWithVideoId, long requestingTweetId, string requestedBy)
            :base(id, DateTime.UtcNow, tweetWithVideoId, requestingTweetId)
        {
            Url = url;
            RequestedBy = requestedBy;
        }

        public TwitterVideo CreateForNewTweet(long requestingTweetId, string requestedBy)
        {
            TwitterVideo video = new TwitterVideo(Guid.NewGuid(), this.Url, this.TweetWithVideoId, requestingTweetId, requestedBy);

            return video;
        }
    }
}
