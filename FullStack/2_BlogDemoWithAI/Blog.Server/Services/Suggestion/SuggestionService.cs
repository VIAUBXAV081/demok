using Blog.Server.Services.Suggestion.Models;

namespace Blog.Server.Services.Suggestion
{
    public abstract class SuggestionService : ExternalServiceBase<SuggestionRequest, SuggestionResponse>, ISuggestionService
    {
        protected abstract string Endpoint { get; }
        protected abstract string Model { get; }

        protected virtual string SystemInstruction => "You are a blogger and you want to write a new post. You have a title in mind, but you are not sure what to write about. You want to generate a short, 1 paragraph blog post content without any formating.";

        private readonly ILogger<OpenAISuggestionService> _logger;

        public SuggestionService(ILogger<OpenAISuggestionService> logger, string apiKey) : base(logger, apiKey)
        {
            _logger = logger;
        }

        public async Task<string> GetSuggestion(string title)
        {
            var request = new SuggestionRequest
            {
                Model = Model,
                Messages = new List<SuggestionMessage>
                    {
                        new SuggestionMessage
                        {
                            Role = "system",
                            Content = SystemInstruction
                        },
                        new SuggestionMessage
                        {
                            Role = "user",
                            Content = title
                        }
                    },
            };

            _logger.LogInformation($"Getting suggestion for {title}");

            var response = await Post(Endpoint, request);

            return response?.Choices?[0].Message?.Content ?? "No suggestion";
        }
    }
}
