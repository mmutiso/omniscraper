using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Omniscraper.Core.TwitterScraper;

namespace Omniscraper.Core.Storage
{
    public class TwitterVideo
    {
        public Guid Id { get; set; }
        public DateTime DateSavedUTC { get; set; }
        public string Slug { get; set; }               
        public string Url { get; set; }
        public long ParentTweetId { get; set; }
        public string VideoThumbnailLinkHttps { get; set; }
        public string Text { get; set; }
        public bool Flagged { get; set; }

        public List<TwitterVideoRequest> TwitterVideoRequests { get; set; }
        public ICollection<VideoTag> VideoTags { get; set; }

        public TwitterVideo()
        {

        }

        public TwitterVideo(Guid id, string url, long parentTweetId, string videoThumbnailLink, string tweetText)
        {
            Id = id;
            DateSavedUTC = DateTime.UtcNow;
            Slug = id.ToString().Split("-")[0];
            Url = url;
            ParentTweetId = parentTweetId;
            VideoThumbnailLinkHttps = videoThumbnailLink;
            Text = tweetText;
        }

        public static TwitterVideo Create(Guid id, VideoResponseModel model, long parentTweetId)
        {
            TwitterVideo twitterVideo = new TwitterVideo(id, model.TwitterPlatformVideoLink,parentTweetId, model.TwitterPlatformThumbnailLink, model.tweetText );

            return twitterVideo;
        }
    }
}
