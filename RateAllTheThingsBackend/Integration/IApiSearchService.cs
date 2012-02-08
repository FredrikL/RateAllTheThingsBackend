using System.Collections.Generic;

namespace RateAllTheThingsBackend.Integration
{
    public interface IApiSearchService
    {
        IEnumerable<ApiSearchHit> Search(string format, string code);
    }
}