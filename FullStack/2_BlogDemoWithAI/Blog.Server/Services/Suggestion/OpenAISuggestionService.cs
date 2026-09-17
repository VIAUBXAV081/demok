namespace Blog.Server.Services.Suggestion
{
    public class OpenAISuggestionService : SuggestionService
    {
        protected override string Endpoint => "https://api.openai.com/v1/chat/completions";
        protected override string Model => "gpt-4o-mini";

        public OpenAISuggestionService(ILogger<OpenAISuggestionService> logger, IConfiguration configuration) : base(logger, configuration["Services:OpenAi:ApiKey"] ?? "") { }
    }
}
