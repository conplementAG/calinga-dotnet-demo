using System.Threading.Tasks;

namespace Calinga.SDK
{
    public interface ILanguageContext
    {
        Task<string> TranslateAsync(string key);
    }
}