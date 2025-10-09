namespace Blog.Server.Services.Suggestion.Models
{
    public record SuggestionRequest
    {
        public required string Model { get; set; }
        public IList<SuggestionMessage>? Messages { get; set; }
        public double Temperature { get; set; } = 0.5;
        public int MaxTokens { get; set; } = 200;
    }
}
