using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Omniscraper.VideosApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        [HttpGet("{tweetId}")]
        public async Task<IActionResult> Index(long? tweetId)
        {
            if(!tweetId.HasValue)
                return BadRequest(new VideoResponseModel(tweetId, string.Empty, false));

            await Task.FromResult(0);
            
            return Ok(new VideoResponseModel(tweetId, "this and that"));
        }
    }
}
