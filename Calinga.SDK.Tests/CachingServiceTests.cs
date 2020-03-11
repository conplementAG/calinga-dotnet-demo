using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

using Calinga.Infrastructure;

namespace Calinga.SDK.Tests
{
    [TestClass]
    public class CachingServiceTests
    {
        private IConsumerHttpClient _consumerHttpClient;
        private ICachingService _cachingService;

        [TestInitialize]
        public void Init()
        {
            _consumerHttpClient = Substitute.For<IConsumerHttpClient>();
            _consumerHttpClient.GetTranslations(TestData.Language_DE).Returns(TestData.Translations_De);
            _consumerHttpClient.GetTranslations(TestData.Language_EN).Returns(TestData.Translations_En);
            _consumerHttpClient.GetLanguages().Returns(TestData.Languages);

            _cachingService = new CachingService(_consumerHttpClient);
        }

        [TestMethod]
        public async Task GetTranslations_ShouldGetTranslationsFromCalingaService()
        {
            // Arrange & Act
            var translations = await _cachingService.GetTranslations(TestData.Language_DE).ConfigureAwait(false);

            // Assert
            translations.ContainsKey(TestData.Key_1).Should().BeTrue();
            translations.ContainsKey(TestData.Key_2).Should().BeTrue();

            await _consumerHttpClient.Received().GetTranslations(TestData.Language_DE).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task GetTranslations_ShouldGetTranslationsFromCache()
        {
            // Arrange
            var translations = await _cachingService.GetTranslations(TestData.Language_DE).ConfigureAwait(false);
            await _consumerHttpClient.Received().GetTranslations(Arg.Any<string>()).ConfigureAwait(false);

            // Act
            var secondCallTranslations = await _cachingService.GetTranslations(TestData.Language_DE).ConfigureAwait(false);

            // Assert
            _consumerHttpClient.DidNotReceive();
            secondCallTranslations.Should().BeSameAs(translations);
        }

        [TestMethod]
        public async Task GetLanguages_ShouldGetTranslationsFromCalingaService()
        {
            // Arrange & Act
            var languages = await _cachingService.GetLanguages().ConfigureAwait(false);

            // Assert
            await _consumerHttpClient.Received().GetLanguages().ConfigureAwait(false);

            Assert.IsNotNull(languages.FirstOrDefault(l => l == TestData.Language_DE));
            Assert.IsNotNull(languages.FirstOrDefault(l => l == TestData.Language_EN));
        }

        [TestMethod]
        public async Task GetLanguages_ShouldGetTranslationsFromCache()
        {
            // Arrange
            var languages = await _cachingService.GetLanguages().ConfigureAwait(false);

            await _consumerHttpClient.Received().GetLanguages().ConfigureAwait(false);

            // Act
            var secondCallLanguages = await _cachingService.GetLanguages().ConfigureAwait(false);

            // Assert
            _consumerHttpClient.DidNotReceive();
            secondCallLanguages.Should().BeSameAs(languages);
        }

        [TestMethod]
        public async Task ClearCache_ShouldClearCache()
        {
            // Arrange
             await _cachingService.GetLanguages().ConfigureAwait(false);
             await _consumerHttpClient.Received().GetLanguages().ConfigureAwait(false);

            // Act
             _cachingService.ClearCache();

            // Assert
            await _consumerHttpClient.Received().GetLanguages().ConfigureAwait(false);
        }
    }
}
