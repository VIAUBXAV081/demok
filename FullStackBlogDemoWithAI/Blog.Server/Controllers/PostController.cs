using Blog.Server.DTOs;
using Blog.Server.Repositories;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
    /// <summary>
    /// Manage posts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(StatusCodeProblemDetails))]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all posts
        /// </summary>
        /// <returns>The list of posts</returns>
        /// <response code="200">Listing successfull</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Post>>> Index()
        {
            _logger.LogInformation("Getting all posts");
            return (await _postRepository.GetPostsAsync()).ToList();
        }

        /// <summary>
        /// Get a post by ID
        /// </summary>
        /// <param name="id">The identifier of post</param>
        /// <returns>The post with give the ID</returns>
        /// <response code="200">Listing successfull</response>
        /// <response code="404">Entity not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Post>> Show(int id)
        {
            _logger.LogInformation($"Getting post with ID {id}");
            return await _postRepository.GetPostAsync(id);
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="post">The details of the post</param>
        /// <returns>The saved details of the post</returns>
        /// <response code="201">Insert successful</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Post>> Create([FromBody] NewPost post)
        {
            _logger.LogInformation($"Creating new post with title {post.Title} and content {post.Content}");
            var created = await _postRepository.InsertPostAsync(post);
            return CreatedAtAction(nameof(Show), new { id = created.ID }, created);
        }

        /// <summary>
        /// Delete a post
        /// </summary>
        /// <param name="id">The identifier of post</param>
        /// <returns>No content</returns>
        /// <response code="204">No content</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Entity not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting post with ID {id}");
            await _postRepository.DeletePostAsync(id);
            return NoContent();
        }
    }
}
