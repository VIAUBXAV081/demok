namespace Blog.Server.DTOs
{
    public record PostTranslationDto
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
