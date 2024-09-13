using Blog.Server.DTOs;
using Blog.Server.Services;
using Blog.Server.Services.Tranlation.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Server.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<PostTranslationDto>> Translate([FromBody] PostTranslationRequestDto post)
        {
            var translations = await _translationService.Translate(new List<string>
            {
                post.Title ?? "",
                post.Content ?? ""
            }, Languages.EN, post.TargetLang);


            return new PostTranslationDto
            {
                Title = translations[0],
                Content = translations[1]
            };
        }
    }
}
