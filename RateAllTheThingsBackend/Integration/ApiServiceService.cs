using System.Collections.Generic;
using System.Threading.Tasks;

namespace RateAllTheThingsBackend.Integration
{
    public class ApiServiceService : IApiSearchService
    {
        private readonly IApiSearch apiSearch;

        public ApiServiceService(IApiSearch apiSearch)
        {
            this.apiSearch = apiSearch;
        }

        public IEnumerable<ApiSearchHit> Search(string format, string code)
        {
            return apiSearch.Search(format, code);
            //return Task<IEnumerable<ApiSearchHit>>.Factory.StartNew(() => apiSearch.Search(format, code)).Result;
        }
    }
}