using System.Collections.Generic;

namespace RateAllTheThingsBackend.Integration
{
    public interface IApiSearch
    {
        IEnumerable<ApiSearchHit> Search(string format, string code);
    }
}