using Blog.Server.DTOs;
using Blog.Server.Services;
using Blog.Server.Services.Tranlation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
    /// <summary>
    /// Translate posts
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly ITranslationService _translationService;

        public TranslationController(ILogger<TranslationController> logger, ITranslationService translationService)
        {
            _logger = logger;
            _translationService = translationService;
        }

        /// <summary>
        /// Translate a post
        /// </summary>
        /// <param name="post">The details of the post</param>
        /// <returns>The translated details of the post</returns>
        /// <response code="200">Listing successful</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PostTranslation>> Translate([FromBody] PostTranslationRequest post)
        {
            var translations = await _translationService.Translate(new List<string>
            {
                post.Title ?? "",
                post.Content ?? ""
            }, Languages.EN, post.TargetLang);


            return new PostTranslation
            {
                Title = translations[0],
                Content = translations[1]
            };
        }
    }
}
