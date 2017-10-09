using System;
using System.Net.Http;
using Wikiled.Core.Standard.Api.Client;

namespace Wikiled.Dictionary.Logic
{
    public class DictionaryManagerFactory
    {
        public IDictionaryManager Construct()
        {
            var client = new HttpClient();
            var manager = new DictionaryManager(new ApiClientFactory(client, new Uri("http://api.wikiled.com")));
            return manager;
        }
    }
}
