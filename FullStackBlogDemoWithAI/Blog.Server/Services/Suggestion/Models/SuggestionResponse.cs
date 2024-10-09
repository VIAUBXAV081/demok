namespace Blog.Server.Services.Suggestion.Models
{
    public record SuggestionResponse
    {
        public IList<SuggestionAnswer>? Choices { get; set; }
    }
}
