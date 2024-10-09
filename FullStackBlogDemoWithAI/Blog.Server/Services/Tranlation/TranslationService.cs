using Blog.Server.Services.Tranlation.Models;

namespace Blog.Server.Services
{
    public class TranslationService : ExternalServiceBase<TranslationRequest, TranslationResponse>, ITranslationService
    {
        private const string _endpoint = "https://api-free.deepl.com/v2/translate";
        private readonly ILogger<TranslationService> _logger;

        public TranslationService(IConfiguration configuration, ILogger<TranslationService> logger) : base(logger, configuration["Services:DeepL:ApiKey"] ?? "", "DeepL-Auth-Key")
        {
            _logger = logger;
        }

        public async Task<string> Translate(string text, Languages sourceLang, Languages targetLang)
        {
            var textList = new List<string> { text };

            var translations = await Translate(textList, sourceLang, targetLang);

            return translations.First();
        }

        public async Task<IList<string>> Translate(IList<string> text, Languages sourceLang, Languages targetLang)
        {
            var request = new TranslationRequest
            {
                SourceLang = sourceLang,
                TargetLang = targetLang,
                Text = text
            };

            _logger.LogInformation($"Getting translation for {text}");

            var response = await Post(_endpoint, request);

            return response?.Translations?.Select((t, i) => t.Text ?? text[i]).ToList() ?? text;
        }

    }

}