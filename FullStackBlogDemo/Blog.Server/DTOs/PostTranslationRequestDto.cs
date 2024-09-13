using Blog.Server.Services.Tranlation.Models;
using System.Text.Json.Serialization;

namespace Blog.Server.DTOs
{
    public record PostTranslationRequestDto
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Languages TargetLang { get; init; }
    }
}
