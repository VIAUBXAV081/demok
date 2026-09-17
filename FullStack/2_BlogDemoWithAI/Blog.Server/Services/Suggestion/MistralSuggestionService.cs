namespace Blog.Server.Services.Suggestion
{
    public class MistralSuggestionService : SuggestionService
    {
        protected override string Endpoint => "https://api.mistral.ai/v1/chat/completions";
        protected override string Model => "mistral-small-latest";

        public MistralSuggestionService(IConfiguration configuration, ILogger<OpenAISuggestionService> logger) : base(logger, configuration["Services:Mistral:ApiKey"] ?? "") { }
    }
}
