using Blog.Server.Services.Tranlation.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Server.DTOs
{
    public record PostTranslationRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required!")]
        [StringLength(450, ErrorMessage = "The title have to be less than or equal to 450 characters long!")]
        public string? Title { get; init; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is required!")]
        public string? Content { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Languages TargetLang { get; init; }
    }
}
