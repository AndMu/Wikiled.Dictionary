using System;
using System.Net.Http;
using Wikiled.Core.Standard.Api;

namespace Wikiled.Dictionary.Logic
{
    public class DictionaryManagerFactory
    {
        public IDictionaryManager Construct()
        {
            var client = new HttpClient
                             {
                                 BaseAddress = new Uri("http://api.wikiled.com")
                             };
            var manager = new DictionaryManager(new ApiClientFactory(client));
            return manager;
        }
    }
}
