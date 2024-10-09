using System.ComponentModel.DataAnnotations;

namespace Blog.Server.DTOs
{
    public record PostTranslation
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
