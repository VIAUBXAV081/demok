using Blog.Server.Services.Tranlation.Models;

namespace Blog.Server.Services
{
    public interface ITranslationService
    {
        Task<string> Translate(string text, Languages sourceLang, Languages targetLang);

        Task<IList<string>> Translate(IList<string> text, Languages sourceLang, Languages targetLang);
    }
}