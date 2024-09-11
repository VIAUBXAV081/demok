using System.Text;
using System.Text.Json;

namespace Blog.Server.Services
{

    public interface ISuggestionService
    {
        Task<string> GetSuggestion(string title);
    }

    public class SuggestionService : ISuggestionService
    {
        private readonly string _apiKey;

        const string endpoint = "https://api.openai.com/v1/chat/completions";
        const string systemInstruction = "You are a blogger and you want to write a new post. You have a title in mind, but you are not sure what to write about. You want to generate a short, 1 paragraph blog post content without any formating.";

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true
        };

        public SuggestionService(IConfiguration configuration)
        {
            _apiKey = configuration["Services:OpenAi:ApiKey"] ?? "";
        }

        public async Task<string> GetSuggestion(string title)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                var content = JsonSerializer.Serialize(GetPrompt(title), _options);
                var response = await client.PostAsync(endpoint, new StringContent(content, Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();
                var suggestion = JsonSerializer.Deserialize<SuggestionResponse>(json, _options);
                return suggestion?.Choices?[0].Message?.Content ?? "No suggestion";
            }
        }

        public SuggestionBody GetPrompt(string title)
        {
            return new SuggestionBody
            {
                Messages = new List<SuggestionMessage>
                    {
                        new SuggestionMessage
                        {
                            Role = "system",
                            Content = systemInstruction
                        },
                        new SuggestionMessage
                        {
                            Role = "user",
                            Content = title
                        }
                    },
            };
        }
    }

    #region Models
    public record SuggestionBody
    {
        public string Model { get; set; } = "gpt-4o-mini";
        public IList<SuggestionMessage>? Messages { get; set; }
        public double Temperature { get; set; } = 0.5;
        public int MaxTokens { get; set; } = 200;
    }

    public record SuggestionMessage
    {
        public string? Role { get; set; }
        public string? Content { get; set; }
    }

    public record SuggestionAnswer
    {
        public SuggestionMessage? Message { get; set; }
    }

    public record SuggestionResponse
    {
        public IList<SuggestionAnswer>? Choices { get; set; }
    }
    #endregion

}
