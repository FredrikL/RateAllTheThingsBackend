using System.Collections.Generic;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IBarCodes
    {
        IEnumerable<BarCode> All();
        BarCode Get(int id);
    }
}