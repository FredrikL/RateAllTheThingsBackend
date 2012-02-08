using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Codeplex.Data;


namespace RateAllTheThingsBackend.Integration
{
    public class GoogleSearchApiForShopping : IApiSearch
    {
        // example: https://www.googleapis.com/shopping/search/v1/public/products?key=KEY&country=US&q=9781905211487
        private const string baseUrl = "https://www.googleapis.com/shopping/search/v1/public/products?key={0}&country=US&q={1}";

        public IEnumerable<ApiSearchHit> Search(string format, string code)
        {
            var result = new ApiSearchHit();
            var url = string.Format(baseUrl, ConfigurationManager.AppSettings["GOOGLE_SEARCH_API_KEY"], code);
            try
            {
                using (var webClient = new WebClient())
                {
                    var data = webClient.DownloadString(url);
                    dynamic x = DynamicJson.Parse(data);
                    if (x.IsDefined("items"))
                    {
                        foreach (dynamic item in x.items)
                        {
                            if (item.product())
                            {
                                var product = item.product;
                                if (product.title() && string.IsNullOrEmpty(result.Name))
                                    result.Name = product.title;
                                if (product.brand() && string.IsNullOrEmpty(result.Manufacturer))
                                    result.Manufacturer = product.brand;
                            }
                        }
                    }
                }
            }catch(Exception)
            {
                // pokemon!
            }

            if (result.IsEmpty) return Enumerable.Empty<ApiSearchHit>();

            return new[] {result};            
        }
    }
}