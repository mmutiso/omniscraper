using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;

namespace Omniscraper.VideosApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        ILogger<VideosController> _logger;
        ITwitterRepository _twitterRepository;
        public VideosController(ILogger<VideosController> logger, TwitterRepositoryFactory twitterRepositoryFactory)
        {
            _logger = logger;
            _twitterRepository = twitterRepositoryFactory.Create();
        }

        [HttpGet("{tweetId}")]
        public async Task<IActionResult> Index(long? tweetId)
        {
            if(!tweetId.HasValue)
                return BadRequest(new VideoResponseModel(tweetId,string.Empty, string.Empty, string.Empty, false));

            try
            {
                RawTweet videoTweet = await _twitterRepository.FindByIdAsync(tweetId.Value);
                TweetNotification tweetNotification = new TweetNotification(videoTweet, default, String.Empty);

                if(tweetNotification?.TweetWithVideo?.ExtendedEntities != null)
                if (tweetNotification.HasVideo())
                {
                    var video = tweetNotification.GetVideo();
                    return Ok(new VideoResponseModel(tweetId, video.Url, video.VideoThumbnailLinkHttps, video.Text));
                }
                return Ok(new VideoResponseModel(tweetId, string.Empty, string.Empty, "no video was found", false));
            }
            catch (Exception ex)
            {
                return Ok(new VideoResponseModel(tweetId, string.Empty, string.Empty, ex.Message, false));
            }
        }
    }
}
