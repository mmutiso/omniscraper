using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Omniscraper.Api.Models;
using Microsoft.AspNetCore.Http;

namespace Omniscraper.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ThreadsController : Controller
    {
        ILogger<ThreadsController> _logger;
        ThreadsService _threadsService;
        public ThreadsController(ILogger<ThreadsController> logger, ThreadsService threadsService)
        {
            _logger = logger;
            _threadsService = threadsService;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery]ThreadRequestModel threadRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(threadRequestModel);
            }

            var thread = await _threadsService
                    .GetAuthorsTweetsInConversationAsync(threadRequestModel.ConversationId, threadRequestModel.AuthorId);

            if (thread.Count == 0)
                return NotFound();

            return Ok(thread.OrderBy(x=>x.ID).Select(x=>x.Text).ToList());
        }
    }
}
