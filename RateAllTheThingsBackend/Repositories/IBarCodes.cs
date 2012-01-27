using System;
using System.Collections.Generic;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IBarCodes
    {
        IEnumerable<BarCode> All();
        BarCode Get(Int64 id, Int64 userId);
        BarCode Get(string format, string code, Int64 userId);
        BarCode Create(string format, string code, Int64 createdBy);
        void Update(BarCode barCode, Int64 updatedBy);
        bool Exists(long barcodeId);
        void Rate(Int64 barCodeId, byte rating, Int64 userid);
        bool HasRated(Int64 userid, Int64 barCodeId);
    }
}