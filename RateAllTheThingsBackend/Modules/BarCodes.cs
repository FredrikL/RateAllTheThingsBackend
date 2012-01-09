using Nancy;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodes:NancyModule
    {
        private readonly IBarCodes barcodes;

        public BarCodes(IBarCodes barcodes)
        {
            this.barcodes = barcodes;

            Get["/BarCode"] = x =>
                                  {
                                      return Response.AsJson(this.barcodes.All());
                                  };

            Get["/BarCode/{format}/{code}"] = x =>
                                {
                                    var barcode = this.barcodes.Get(x.format, x.code);
                                    return Response.AsJson(new[] { barcode });
                                };
        }
    }
}