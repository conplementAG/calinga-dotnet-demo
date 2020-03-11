using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calinga.Infrastructure
{
    public interface IConsumerHttpClient
    {
        Task<IReadOnlyDictionary<string, string>> GetTranslations(string language);

        Task<IEnumerable<string>> GetLanguages();
    }
}