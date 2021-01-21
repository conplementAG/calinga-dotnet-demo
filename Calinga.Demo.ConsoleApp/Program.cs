using static System.FormattableString;

using System;
using System.Linq;
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

            languages.ToList().ForEach(Console.WriteLine);

            Console.WriteLine("calinga service: try to get translations");

            var translation = await service.TranslateAsync("Button.Cancel", "en").ConfigureAwait(false);

            Console.WriteLine(Invariant($"Key: Button.Cancel,  Translation: {translation}"));

             translation = await service.TranslateAsync("Button.Create", "en").ConfigureAwait(false);

            Console.WriteLine(Invariant($"Key: Button.Create,  Translation: {translation}"));

            Console.WriteLine("clean cache...");
            service.ClearCache();

            Console.ReadKey(false);
        }

        private static CalingaServiceSettings AppCalingaServiceSettings => new CalingaServiceSettings
        {
            Organization = "SdkSample",

            Project = "ExampleProject",

            ApiToken = "dd60c24b1353f14fc614742e1a9c687a",

            Team = "Default Team",

            IsDevMode = false,

            CacheDirectory = "CacheFiles"
        };
    }
}
