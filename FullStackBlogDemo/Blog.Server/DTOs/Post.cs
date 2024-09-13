namespace Blog.Server.DTOs
{
    public record Post
    {
        public int? ID { get; init; }
        public string? Title { get; init; }
        public string? Content { get; init; }
    }
}
