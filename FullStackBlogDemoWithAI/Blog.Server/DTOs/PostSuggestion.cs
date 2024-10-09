namespace Blog.Server.DTOs
{
    public record PostSuggestion
    {
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
