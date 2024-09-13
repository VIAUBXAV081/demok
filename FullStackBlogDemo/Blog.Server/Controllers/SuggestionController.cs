using Blog.Server.DTOs;
using Blog.Server.Services.Suggestion;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<PostSuggestionDto>> Suggest([FromBody] NewPostIdeaDto post)
        {
            return new PostSuggestionDto
            {
                Title = post.Title,
                Content = await _suggestionService.GetSuggestion(post.Title ?? "")
            };
        }
    }
}
