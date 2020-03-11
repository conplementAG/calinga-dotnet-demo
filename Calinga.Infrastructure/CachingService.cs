using static System.FormattableString;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Calinga.Infrastructure
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _translationsCache;
        private readonly IConsumerHttpClient _consumerHttpClient;
        private readonly ConcurrentDictionary<string, bool> _allKeys = new ConcurrentDictionary<string, bool>();
        
        private const string LanguagesCacheKey = "Languages";

        public CachingService(IConsumerHttpClient consumerHttpClient)
        {
            _consumerHttpClient = consumerHttpClient;
            _translationsCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task<IReadOnlyDictionary<string, string>> GetTranslations(string language)
        {
            var cacheKey = Invariant($"Language_{language}");
            _translationsCache.TryGetValue(cacheKey, out var translations);

            if (translations == null)
            {
                translations = await _consumerHttpClient.GetTranslations(language).ConfigureAwait(false);
                StoreInCache(translations, cacheKey);
            }

            return translations as IReadOnlyDictionary<string, string>;
        }

        public async Task<IEnumerable<string>> GetLanguages()
        {
            var languages = _translationsCache.Get(LanguagesCacheKey);

            if (languages == null)
            {
                languages = await _consumerHttpClient.GetLanguages().ConfigureAwait(false);
                StoreInCache(languages, LanguagesCacheKey);
            }

            return languages as IEnumerable<string>;
        }

        public void ClearCache()
        {
            CachedKeys.ToList().ForEach(k => _translationsCache.Remove(RemoveKey(k)));
            ClearKeys();
        }

        #region Private helper Methods

        private IEnumerable<string> CachedKeys => _allKeys.Keys;

        private void StoreInCache(object data, string cacheKey)
        {
            _allKeys.TryAdd(cacheKey, true);
            _translationsCache.Set(cacheKey, data, new MemoryCacheEntryOptions());
        }

        private string RemoveKey(string key)
        {
            if (!_allKeys.TryRemove(key, out _))
                //if not possible to remove key from dictionary, then try to mark key as not existing in cache
                _allKeys.TryUpdate(key, false, true);

            return key;
        }

        private void ClearKeys()
        {
            foreach (var key in _allKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                RemoveKey(key);
            }
        }

        #endregion Private helper Methods
    }
}
