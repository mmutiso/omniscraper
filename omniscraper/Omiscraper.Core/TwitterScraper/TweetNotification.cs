using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omiscraper.Core.TwitterScraper
{
    public class TweetNotification
    {
        Status tweet;
        public TweetNotification(Status tweetNotification)
        {
            tweet = tweetNotification;
        }

        public bool HasVideo()
        {
            if (tweet != null)
            {
                var extendedEntities = tweet.ExtendedEntities;
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

        public List<TweetVideoInfo> GetVideoLinks()
        {
            List<TweetVideoInfo> videoLinks = new List<TweetVideoInfo>(0);
            if (tweet != null)
            {
                var extendedEntities = tweet.ExtendedEntities;
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
    }
}
