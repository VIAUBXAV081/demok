using Blog.Server.DTOs;
using Blog.Server.Repositories;
using Blog.Server.Services.Suggestion;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> Index()
        {
            _logger.LogInformation("Getting all posts");
            return (await _postRepository.GetPostsAsync()).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> Show(int id)
        {
            _logger.LogInformation($"Getting post with ID {id}");
            return await _postRepository.GetPostAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> Create([FromBody] NewPostDto post)
        {
            _logger.LogInformation($"Creating new post with title {post.Title} and content {post.Content}");
            var created = await _postRepository.InsertPostAsync(post);
            return CreatedAtAction(nameof(Show), new { id = created.ID }, created);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting post with ID {id}");
            await _postRepository.DeletePostAsync(id);
            return NoContent();
        }
    }
}
