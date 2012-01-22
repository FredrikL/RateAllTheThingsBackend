using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodesModule : NancyModule
    {
        private readonly IBarCodes barCodes;
        private readonly IUsers users;

        public BarCodesModule(IBarCodes barCodes, IUsers users)
            : base("/BarCode")
        {
            this.RequiresAuthentication();

            this.barCodes = barCodes;
            this.users = users;

            Get["/"] = x =>
                           {
                               return Response.AsJson(this.barCodes.All());
                           };

            Get["/{id}"] = x =>
            {
                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                var barcode = this.barCodes.Get(x.id, userId);
                return Response.AsJson(new[] { barcode });
            };

            Get["/{format}/{code}"] = x =>
                                          {
                                              var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                              var barcode = this.barCodes.Get(x.format, x.code, userId) ?? this.barCodes.Create(x.format, x.code, userId);
                                              return Response.AsJson(new[] {barcode});
                                          };
            Post["/"] = x =>
                            {
                                BarCode barCode = this.Bind<BarCode>();
                                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                var originalCode = this.barCodes.Get(barCode.Id, userId);
                                if (originalCode.Format == barCode.Format && originalCode.Code == barCode.Code)
                                {
                                    // stuff that we allow to be updated
                                    originalCode.Name = barCode.Name;
                                    barCode = this.barCodes.Update(originalCode, userId);
                                }
                                return Response.AsJson(new[] {barCode});
                            };

            Post["/Rate/{id}/{value}"] = x =>
                                             {
                                                 var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);

                                                 if (!this.barCodes.Exists(x.id))
                                                     return Response.AsJson(Enumerable.Empty<BarCode>()); // return 404 ?
                                                 if(x.value < 1 || x.value > 6)
                                                     return Response.AsJson(Enumerable.Empty<BarCode>()); // return xxx ?

                                                 if(!this.barCodes.HasRated(userId, x.id))                                                    
                                                    this.barCodes.Rate(x.id, (byte)x.value, userId);

                                                 return Response.AsJson(new[] {this.barCodes.Get(x.id, userId)});
                                             };
        }

    }
}