﻿namespace Omniscraper.Core.TwitterScraper
{ 
    public record VideoResponseModel
    {
        public long? TweetId { get; set; }
        public string TwitterPlatformVideoLink { get; set; }
        public bool FoundVideo { get; set; }
        public string tweetText { get; set; }
        public string TwitterPlatformThumbnailLink { get; set; }
        public bool PotentiallySensitive { get; set; }

        public VideoResponseModel()
        {
        }

        public VideoResponseModel(long? tweetId, string videoLink, string thumbnailLink, string text, bool found = true, bool sensitive = false)
        {
            TweetId = tweetId;
            TwitterPlatformVideoLink = videoLink;
            FoundVideo = found;
            PotentiallySensitive = sensitive;
            TwitterPlatformThumbnailLink = thumbnailLink;
            tweetText = text;
        }
    }
}
