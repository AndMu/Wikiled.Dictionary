using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using Wikiled.Core.Standard.Api;
using Wikiled.Dictionary.Data;
using Wikiled.Dictionary.Logic;

namespace Wikiled.Dictionary.Tests.Logic
{
    [TestFixture]
    public class DictionaryManagerTests
    {
        private Mock<IApiClientFactory> mockApiClientFactory;

        private DictionaryManager instance;

        [SetUp]
        public void Setup()
        {
            mockApiClientFactory = new Mock<IApiClientFactory>();
            instance = CreateManager();
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new DictionaryManager(null));
        }

        [Test]
        public void TranslateArguments()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => instance.Translate(null, CancellationToken.None));
            TranslationRequest request = new TranslationRequest();
            request.From = Language.English;
            request.To = Language.Danish;
            Assert.ThrowsAsync<ArgumentException>(() => instance.Translate(request, CancellationToken.None));
            request.Word = "Test";
            request.From = Language.Spanish;
            request.To = Language.Danish;
            Assert.ThrowsAsync<ArgumentException>(() => instance.Translate(request, CancellationToken.None));
            request.From = Language.English;
            request.To = Language.English;
            Assert.ThrowsAsync<ArgumentException>(() => instance.Translate(request, CancellationToken.None));
        }

        private DictionaryManager CreateManager()
        {
            return new DictionaryManager(mockApiClientFactory.Object);
        }
    }
}
