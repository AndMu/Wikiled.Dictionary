using System;
using Wikiled.Core.Standard.Api;

namespace Wikiled.Dictionary.Logic
{
    public class DictionaryManager
    {
        private readonly IApiClientFactory factory;

        public DictionaryManager(IApiClientFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException();
        }
    }
}
