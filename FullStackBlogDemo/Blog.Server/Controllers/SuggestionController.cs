using Blog.Server.DTOs;
using Blog.Server.Services.Suggestion;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
    /// <summary>
    /// Suggests a post idea
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly ILogger<SuggestionController> _logger;
        private readonly ISuggestionService _suggestionService;

        public SuggestionController(ILogger<SuggestionController> logger, ISuggestionService suggestionService)
        {
            _logger = logger;
            _suggestionService = suggestionService;
        }

        /// <summary>
        /// Suggest a post idea
        /// </summary>
        /// <param name="post">The initial details of post</param>
        /// <returns>Post suggestion</returns>
        /// <response code="200">Listing successful</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostSuggestion>> Suggest([FromBody] NewPostIdea post)
        {
            return new PostSuggestion
            {
                Title = post.Title,
                Content = await _suggestionService.GetSuggestion(post.Title ?? "")
            };
        }
    }
}
