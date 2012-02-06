using System;
using System.Collections.Generic;
using System.Configuration;

namespace RateAllTheThingsBackend.Integration
{
    public class GoogleSearchApiForShopping : IApiSearch
    {
        // example: https://www.googleapis.com/shopping/search/v1/public/products?key=KEY&country=US&q=9781905211487
        private const string baseUrl = "https://www.googleapis.com/shopping/search/v1/public/products?key={0}&country=US&q={1}";

        public IEnumerable<ApiSearchHit> Search(string format, string code)
        {
            var url = string.Format(baseUrl, ConfigurationManager.AppSettings["GOOGLE_SEARCH_API_KEY"], code);
            throw new NotImplementedException();
        }
    }
}