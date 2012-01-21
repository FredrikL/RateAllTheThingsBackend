using System.Collections.Generic;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IBarCodes
    {
        IEnumerable<BarCode> All();
        BarCode Get(long id);
        BarCode Get(string format, string code);
        BarCode Create(string format, string code, long createdBy);
        BarCode Update(BarCode barCode, long updatedBy);
        bool Exists(long barcodeId);
    }
}