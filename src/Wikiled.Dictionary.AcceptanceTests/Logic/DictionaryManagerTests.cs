using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Wikiled.Core.Standard.Api;
using Wikiled.Core.Standard.Api.Client;
using Wikiled.Dictionary.Data;
using Wikiled.Dictionary.Logic;

namespace Wikiled.Dictionary.AcceptanceTests.Logic
{
    [TestFixture]
    public class DictionaryManagerTests
    {
        private DictionaryManager instance;

        [SetUp]
        public void Setup()
        {
            instance = CreateManager();
        }

        [TestCase("Love", 8)]
        [TestCase("King", 2)]
        [TestCase("xxx", 0)]
        public async Task Translate(string word, int total)
        {
            var result = await instance.Translate(
                             new TranslationRequest
                             {
                                 From = Language.English,
                                 To = Language.German,
                                 Word = word
                             },
                             CancellationToken.None).ConfigureAwait(false);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(total, result.Result.Translations.Length);
        }

        private DictionaryManager CreateManager()
        {
            return new DictionaryManager(new ApiClientFactory(new HttpClient(), new Uri("http://api.wikiled.com")));
        }
    }
}
