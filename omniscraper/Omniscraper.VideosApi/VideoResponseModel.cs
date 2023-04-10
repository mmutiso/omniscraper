namespace Omniscraper.VideosApi
{
    public record VideoResponseModel
    {
        public long? TweetId { get; set; }
        public string TwitterPlatformVideoLink { get; set; }
        public bool FoundVideo { get; set; }

        public VideoResponseModel(long? tweetId, string videoLink, bool found = true)
        {
            TweetId = tweetId;
            TwitterPlatformVideoLink = videoLink;
            FoundVideo = found;
        }
    }
}
