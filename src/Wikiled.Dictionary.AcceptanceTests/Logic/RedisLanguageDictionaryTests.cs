using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Wikiled.Dictionary.Data;
using Wikiled.Dictionary.Logic;
using Wikiled.Redis.Config;
using Wikiled.Redis.Logic.Pool;

namespace Wikiled.Dictionary.AcceptanceTests.Logic
{
    [TestFixture]
    public class RedisLanguageDictionaryTests
    {
        private IRedisLinksPool redisLinksPool;

        private RedisLanguageDictionary instance;

        [SetUp]
        public void SetUp()
        {
            redisLinksPool = new RedisLinksPool(
                new[]
            {
                new RedisConfiguration("localhost", 6370)
                {
                    ServiceName = "D"
                }
            });
            redisLinksPool.Open();
            instance = CreateInstance();
        }

        [TearDown]
        public void TearDown()
        {
            redisLinksPool.Dispose();
        }

        [Test]
        public async Task Translate()
        {
            var result = await instance.Translate(
                new TranslationRequest
                {
                    From = Language.English,
                    To = Language.Lithuanian,
                    Word = "Mother"
                });

            Assert.AreEqual(16, result.Translations.Length);
        }


        [Test]
        public async Task TranslateCross()
        {
            var result = await instance.Translate(
                new TranslationRequest
                {
                    From = Language.Lithuanian,
                    To = Language.Russian,
                    Word = "Motina"
                });

            Assert.AreEqual(16, result.Translations.Length);
        }

        private RedisLanguageDictionary CreateInstance()
        {
            return new RedisLanguageDictionary(redisLinksPool);
        }
    }
}
