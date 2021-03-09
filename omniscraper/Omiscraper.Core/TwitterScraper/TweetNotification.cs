using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using Omniscraper.Core.Storage;

namespace Omniscraper.Core.TwitterScraper
{
    public class TweetNotification
    {
        public RawTweetv2 TweetWithVideo;
        long RequestingTweetId;
        Guid Id;
        string RequestedBy;

        

        public TweetNotification(RawTweetv2 tweetWithVideo, long requestingTweetId, string requestedBy)
        {
            TweetWithVideo = tweetWithVideo;
            RequestingTweetId = requestingTweetId;
            Id = Guid.NewGuid();
            RequestedBy = requestedBy;
        }

        public TwitterVideo GetVideo()
        {
            if (HasVideo())
            {
                long idOfTweetWithVideo = long.Parse(TweetWithVideo.Id);

                var video = new TwitterVideo(Id, GetVideoUrl(), idOfTweetWithVideo, RequestingTweetId, RequestedBy);
                return video;
            }
            return null;
        }

        

        public bool HasVideo()
        {
            
            return false;
        }

        private List<TweetVideoLink> GetVideoLinks()
        {
            List<TweetVideoLink> videoLinks = new List<TweetVideoLink>(0);
            if (TweetWithVideo != null)
            {
                return default;
                
            }
            return videoLinks;
        }

        /// <summary>
        /// The logic for inferring intent will all come here. In the mean time everything 
        /// will default to video. However in the future the priority will be given to determining if
        /// intent is on a thread since it might have some tweets with videos.
        /// </summary>
        /// <returns></returns>
        public TwitterContentType GetContentType()
        {
            if (HasVideo())
                return TwitterContentType.Video;
            else
                return TwitterContentType.Undefined;                
        }

        public string GetVideoUrl()
        {
            var videoLinks = GetVideoLinks();
            int maxBitRate = -1;
            string videoLink = string.Empty;
            videoLinks.ForEach(link =>
            {
                if (link.BitRate > maxBitRate)
                {
                    videoLink = link.Url;
                    maxBitRate = link.BitRate;
                }
            });

            return videoLink;
        }
    }
}
