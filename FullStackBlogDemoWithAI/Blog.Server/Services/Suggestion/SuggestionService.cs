using Blog.Server.Services.Suggestion.Models;

namespace Blog.Server.Services.Suggestion
{
    public class SuggestionService : ExternalServiceBase<SuggestionRequest, SuggestionResponse>, ISuggestionService
    {
        private const string _endpoint = "https://api.openai.com/v1/chat/completions";
        private const string _systemInstruction = "You are a blogger and you want to write a new post. You have a title in mind, but you are not sure what to write about. You want to generate a short, 1 paragraph blog post content without any formating.";
        private readonly ILogger<SuggestionService> _logger;

        public SuggestionService(IConfiguration configuration, ILogger<SuggestionService> logger) : base(logger, configuration["Services:OpenAi:ApiKey"] ?? "")
        {
            _logger = logger;
        }

        public async Task<string> GetSuggestion(string title)
        {
            var request = new SuggestionRequest
            {
                Messages = new List<SuggestionMessage>
                    {
                        new SuggestionMessage
                        {
                            Role = "system",
                            Content = _systemInstruction
                        },
                        new SuggestionMessage
                        {
                            Role = "user",
                            Content = title
                        }
                    },
            };

            _logger.LogInformation($"Getting suggestion for {title}");

            var response = await Post(_endpoint, request);

            return response?.Choices?[0].Message?.Content ?? "No suggestion";
        }
    }
}
