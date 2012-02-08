using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Controller;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodesModule : NancyModule
    {
        private readonly IUsers users;
        private readonly IEventLog eventLog;
        private readonly IBarCodeController barCodeController;

        public BarCodesModule(IUsers users,
            IEventLog eventLog, IBarCodeController barCodeController)
            : base("/BarCode")
        {
            this.RequiresAuthentication();

            this.users = users;
            this.eventLog = eventLog;
            this.barCodeController = barCodeController;

            Get["/{id}"] = x =>
            {
                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);                
                var barcode = this.barCodeController.Get(x.id, userId);
                this.Log(barcode, userId, "GET");
                return Response.AsJson(new[] { barcode });
            };

            Get["/{format}/{code}"] = x =>
                                          {                                              
                                              if (x.format == null || x.code == null)
                                                  return Response.AsJson(Enumerable.Empty<BarCode>());

                                              var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                              BarCode barcode = this.barCodeController.Get(x.format, x.code, userId);
                                              this.Log(barcode, userId, "GET");
                                              return Response.AsJson(new[] {barcode});
                                          };
            Post["/"] = x =>
                            {
                                BarCode barCode = this.Bind<BarCode>();
                                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                var originalCode = this.barCodeController.Update(barCode, userId);
                                this.Log(originalCode, userId, "UPDATE", barCode.Name);
                                return Response.AsJson(new[] { originalCode });
                            };

            Post["/Rate/{id}/{value}"] = x =>
                                             {
                                                 var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                                 var code = this.barCodeController.Rate(x.id, (Byte)x.value, userId);
                                                 this.Log(x.id, userId, "RATE");
                                                 return Response.AsJson(new[] { code });
                                             };
        }

        private void Log(BarCode barcode, long userId, string eventName,string data = null)
        {
            this.Log(barcode.Id, userId, eventName, data);
        }

        private void Log(long barcodeId, long userId, string eventName,string data = null)
        {
            this.eventLog.LogEvent(new Event()
            {
                AuthorId = userId,
                BarCodeId = barcodeId,
                Ip = this.Request.Headers["X-Forwarded-For"].FirstOrDefault(),
                EventName = eventName,
                Data = null
            });
        }
    }
}