namespace Blog.Server.Services.Suggestion.Models
{
    public record SuggestionMessage
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }
}
