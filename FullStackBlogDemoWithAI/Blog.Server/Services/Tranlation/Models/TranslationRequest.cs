using System.Text.Json.Serialization;

namespace Blog.Server.Services.Tranlation.Models
{
    public record TranslationRequest
    {
        public IList<string>? Text { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Languages? SourceLang { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Languages? TargetLang { get; set; }
    }
}