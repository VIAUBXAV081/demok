namespace Blog.Server.Services.Suggestion
{
    public interface ISuggestionService
    {
        Task<string> GetSuggestion(string title);
    }
}
