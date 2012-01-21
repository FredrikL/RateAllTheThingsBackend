using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodesModule : NancyModule
    {
        private readonly IBarCodes barcodes;
        private readonly IUsers users;

        public BarCodesModule(IBarCodes barcodes, IUsers users)
            : base("/BarCode")
        {
            this.RequiresAuthentication();

            this.barcodes = barcodes;
            this.users = users;

            Get["/"] = x =>
                           {
                               return Response.AsJson(this.barcodes.All());
                           };

            Get["/{format}/{code}"] = x =>
                                          {
                                              var barcode = this.barcodes.Get(x.format, x.code) ?? this.barcodes.Create(x.format, x.code, this.users.GetIdByUsername(this.Context.CurrentUser.UserName));
                                              return Response.AsJson(new[] {barcode});
                                          };
            Post["/"] = x =>
                            {
                                BarCode barCode = this.Bind<BarCode>();
                                var originalCode = this.barcodes.Get(barCode.Id);
                                if (originalCode.Format == barCode.Format && originalCode.Code == barCode.Code)
                                {
                                    // stuff that we allow to be updated
                                    originalCode.Name = barCode.Name;

                                    barCode = this.barcodes.Update(originalCode, this.users.GetIdByUsername(this.Context.CurrentUser.UserName));
                                }
                                return Response.AsJson(new[] {barCode});
                            };

            Post["/Rate/{id}/{value}"] = x =>
                                             {
                                                 throw new NotImplementedException();
                                             };
        }

    }
}