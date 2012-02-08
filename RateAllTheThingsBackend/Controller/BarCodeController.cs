using System.Collections.Generic;
using System.Linq;
using RateAllTheThingsBackend.Integration;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controller
{
    public class BarCodeController : IBarCodeController
    {
        private readonly IBarCodes barCodes;
        private readonly IApiSearchService apiSearchService;

        public BarCodeController(IBarCodes barCodes, IApiSearchService apiSearchService)
        {
            this.barCodes = barCodes;
            this.apiSearchService = apiSearchService;
        }

        public BarCode Get(long id, long userId)
        {
            return this.barCodes.Get(id, userId);
        }

        public BarCode Get(string format, string code, long userId)
        {
            BarCode barcode = this.barCodes.Get(format, code, userId) ?? this.barCodes.Create(format, code, userId);
            if (barcode.New)
            {
                IEnumerable<ApiSearchHit> searchHits = this.apiSearchService.Search(format, code);
                if (searchHits.Any())
                {
                    var first = searchHits.First();
                    barcode.Name = first.Name;
                    barcode.Manufacturer = first.Manufacturer;
                    this.barCodes.Update(barcode, userId);
                }
            }
            return barcode;
        }

        public BarCode Update(BarCode barCode, long userId)
        {
              var originalCode = this.barCodes.Get(barCode.Id, userId);
              if (originalCode.Format == barCode.Format && originalCode.Code == barCode.Code)
              {
                  // stuff that we allow to be updated
                  originalCode.Name = barCode.Name;
                  originalCode.Manufacturer = barCode.Manufacturer;

                  this.barCodes.Update(originalCode, userId);
              }
            return originalCode;
        }

        public BarCode Rate(long id, byte rating, long userId)
        {
            if (!this.barCodes.Exists(id) || (rating < 1 || rating > 6))
                return this.barCodes.Get(id, userId);

            if (!this.barCodes.HasRated(userId, id))
            {
                this.barCodes.Rate(id, rating, userId);
            }
            return this.barCodes.Get(id, userId);
        }
    }
}