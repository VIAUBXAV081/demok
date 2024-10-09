using System.Text;
using System.Text.Json;

namespace Blog.Server.Services
{
    public abstract class ExternalServiceBase<TRequest, TResponse>
    {
        private readonly string _apiKey;
        private readonly string _authHeaderFormat;
        private readonly ILogger<ExternalServiceBase<TRequest, TResponse>> _logger;

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = true
        };

        protected ExternalServiceBase(ILogger<ExternalServiceBase<TRequest, TResponse>> logger, string apiKey, string authHeaderFormat = "Bearer", JsonSerializerOptions? options = null)
        {
            _apiKey = apiKey;
            _authHeaderFormat = authHeaderFormat;
            _options = options ?? _options;
            _logger = logger;
        }

        public async Task<TResponse?> Post(string endpoint, TRequest request)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"{_authHeaderFormat} {_apiKey}");

                var content = JsonSerializer.Serialize(request, _options);

                var response = await client.PostAsync(endpoint, new StringContent(content, Encoding.UTF8, "application/json"));
                _logger.LogInformation($"Called {endpoint}. Status code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to call {endpoint}. Status code: {response.StatusCode}");
                    return default;
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(json, _options);
            }
        }
    }
}
