using System;
using Moq;
using NUnit.Framework;
using Wikiled.Core.Standard.Api;
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

        private DictionaryManager CreateManager()
        {
            return new DictionaryManager(mockApiClientFactory.Object);
        }
    }
}
