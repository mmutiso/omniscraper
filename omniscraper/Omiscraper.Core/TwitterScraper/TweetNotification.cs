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
        public RawTweet TweetWithVideo;
        long RequestingTweetId;
        Guid Id;
        string RequestedBy;

        

        public TweetNotification(RawTweet tweetWithVideo, long requestingTweetId, string requestedBy)
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
                var video = new TwitterVideo(Id, GetVideoUrl(), TweetWithVideo.id);
                return video;
            }
            return null;
        }

        public TwitterVideoRequest GetVideoRequest(Guid videoId)
        {
            var request = new TwitterVideoRequest(RequestingTweetId, videoId, RequestedBy);

            return request;
        }

        public bool HasVideo()
        {
            if (TweetWithVideo != null)
            {
                var extendedEntities = TweetWithVideo.ExtendedEntities;
                if (extendedEntities != null)
                {
                    var mediaEntities = extendedEntities.MediaEntities;
                    if (mediaEntities.Count > 0)
                    {
                        var videoInfo = mediaEntities[0].VideoInfo;
                        if (videoInfo != null)
                        {
                            var variants = videoInfo.Variants;
                            if (variants.Count > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private List<TweetVideoLink> GetVideoLinks()
        {
            List<TweetVideoLink> videoLinks = new List<TweetVideoLink>(0);
            if (TweetWithVideo != null)
            {
                var extendedEntities = TweetWithVideo.ExtendedEntities;
                if (extendedEntities != null)
                {
                    var mediaEntities = extendedEntities.MediaEntities;
                    if (mediaEntities.Count > 0)
                    {
                        var videoInfo = mediaEntities[0].VideoInfo;
                        if (videoInfo != null)
                        {
                            var variants = videoInfo.Variants;
                            if (variants.Count > 0)
                            {
                                videoLinks = variants.GetVideoLinks();
                                return videoLinks;
                            }
                        }
                    }
                }
            }
            return videoLinks;
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
