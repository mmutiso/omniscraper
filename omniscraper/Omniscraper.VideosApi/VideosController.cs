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
                return BadRequest(new VideoResponseModel(tweetId, string.Empty, false));

            try
            {
                RawTweet videoTweet = await _twitterRepository.FindByIdAsync(tweetId.Value);
                TweetNotification tweetNotification = new TweetNotification(videoTweet, default, default);

                if (tweetNotification.HasVideo())
                {
                    var video = tweetNotification.GetVideo();
                    return Ok(new VideoResponseModel(tweetId, video.Url));
                }
                return Ok(new VideoResponseModel(tweetId, "No video was found", false));
            }
            catch (Exception ex)
            {
                return Ok(new VideoResponseModel(tweetId, ex.Message, false));
            }
        }
    }
}
