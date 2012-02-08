using System;
using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Controller
{
    public interface IBarCodeController
    {
        BarCode Get(Int64 id, Int64 userId);
        BarCode Get(string format, string code, Int64 userId);
        BarCode Update(BarCode code, Int64 userId);
        BarCode Rate(Int64 id, byte rating, Int64 userId);
    }
}