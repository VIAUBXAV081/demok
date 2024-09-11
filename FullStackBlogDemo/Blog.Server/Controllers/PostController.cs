using Blog.Server.DTOs;
using Blog.Server.Repositories;
using Blog.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;
        private readonly ISuggestionService _suggestionService;


        public PostController(IPostRepository postRepository, ILogger<PostController> logger, ISuggestionService suggestionService)
        {
            _postRepository = postRepository;
            _logger = logger;
            _suggestionService = suggestionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> Index()
        {
            return (await _postRepository.GetPostsAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Show(int id)
        {
            return await _postRepository.GetPostAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> Create([FromBody] NewPostDto post)
        {
            var created = await _postRepository.InsertPostAsync(post);
            return CreatedAtAction(nameof(Show), new { id = created.ID }, created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _postRepository.DeletePostAsync(id);
            return NoContent();
        }

        [HttpPost]
        [Route("suggest")]
        public async Task<ActionResult<PostSuggestionDto>> Suggest([FromBody] NewPostIdeaDto post)
        {
            return new PostSuggestionDto
            {
                Title = post.Title,
                Content = await _suggestionService.GetSuggestion(post.Title)
            };

        }
    }
}
