using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calinga.SDK
{
    public interface  ICalingaService
    {
        Task<string> TranslateAsync(string key, string language);

        Task<IEnumerable<string>> GetLanguagesAsync();

        ILanguageContext CreateContext(string language);

        void ClearCacheAsync();
    }
}