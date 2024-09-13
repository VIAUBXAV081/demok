namespace Blog.Server.DTOs
{
    public record PostSuggestionDto
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
