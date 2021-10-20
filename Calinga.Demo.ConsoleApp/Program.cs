using static System.FormattableString;

using System;
using System.Threading.Tasks;

using Calinga.NET;
using Calinga.NET.Infrastructure;

namespace Calinga.Demo.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main()
        {
            var service = new CalingaService(AppCalingaServiceSettings);

            Console.WriteLine("calinga service: try to get all languages");

            var languages = await service.GetLanguagesAsync().ConfigureAwait(false);

            foreach (var name in languages)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine("calinga service: get reference language");
            Console.WriteLine(await service.GetReferenceLanguage());

            Console.WriteLine("calinga service: try to get translations");

            var translation = await service.TranslateAsync("Button.Cancel", "en").ConfigureAwait(false);

            Console.WriteLine(Invariant($"Key: Button.Cancel,  Translation: {translation}"));

            translation = await service.TranslateAsync("Button.Create", "en").ConfigureAwait(false);

            Console.WriteLine(Invariant($"Key: Button.Create,  Translation: {translation}"));

            Console.WriteLine("clean cache...");
            await service.ClearCache().ConfigureAwait(false);

            Console.ReadKey(false);
        }

        private static CalingaServiceSettings AppCalingaServiceSettings => new CalingaServiceSettings
        {
            Organization = "SdkSample",

            Project = "ExampleProject",

            ApiToken = "d9bd4a4ab94a9a1e4dc0fa7bdfafb123",

            Team = "Default Team",

            IsDevMode = false,

            CacheDirectory = "CacheFiles",

            MemoryCacheExpirationIntervalInSeconds = 600,

            DoNotWriteCacheFiles = false
        };
    }
}
