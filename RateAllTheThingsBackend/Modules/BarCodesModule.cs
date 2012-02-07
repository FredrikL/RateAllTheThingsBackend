﻿using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using RateAllTheThingsBackend.Integration;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Modules
{
    public class BarCodesModule : NancyModule
    {
        private readonly IBarCodes barCodes;
        private readonly IUsers users;
        private readonly IEventLog eventLog;
        private readonly IApiSearch apiSearch;

        public BarCodesModule(IBarCodes barCodes, IUsers users,
            IEventLog eventLog, IApiSearch apiSearch)
            : base("/BarCode")
        {
            this.RequiresAuthentication();

            this.barCodes = barCodes;
            this.users = users;
            this.eventLog = eventLog;
            this.apiSearch = apiSearch;

            Get["/{id}"] = x =>
            {
                var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                var barcode = this.barCodes.Get(x.id, userId);
                this.Log(barcode, userId, "GET");
                return Response.AsJson(new[] { barcode });
            };

            Get["/{format}/{code}"] = x =>
                                          {                                              
                                              var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);
                                              if (x.format != null && x.code != null)
                                              {
                                                  BarCode barcode = this.barCodes.Get(x.format, x.code, userId) ?? this.barCodes.Create(x.format, x.code, userId);
                                                  if(barcode.New)
                                                  {
                                                      IEnumerable<ApiSearchHit> searchHits = this.apiSearch.Search(x.format, x.code);
                                                      if(searchHits.Any())
                                                      {
                                                          var first = searchHits.First();
                                                          barcode.Name = first.Name;
                                                          barcode.Manufacturer = first.Manufacturer;
                                                          this.barCodes.Update(barcode, userId);
                                                      }
                                                  }
                                                  this.Log(barcode, userId, "GET");

                                                  return Response.AsJson(new[] {barcode});
                                              }
                                              return Response.AsJson(Enumerable.Empty<BarCode>());
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
                                    originalCode.Manufacturer = barCode.Manufacturer;
                                    this.barCodes.Update(originalCode, userId);

                                    this.Log(originalCode, userId, "UPDATE", barCode.Name);
                                }
                                return Response.AsJson(new[] { originalCode });
                            };

            Post["/Rate/{id}/{value}"] = x =>
                                             {
                                                 var userId = this.users.GetIdByUsername(this.Context.CurrentUser.UserName);

                                                 if (!this.barCodes.Exists(x.id))
                                                     return Response.AsJson(Enumerable.Empty<BarCode>()); // return 404 ?
                                                 if(x.value < 1 || x.value > 6)
                                                     return Response.AsJson(Enumerable.Empty<BarCode>()); // return xxx ?

                                                 if (!this.barCodes.HasRated(userId, x.id))
                                                 {
                                                     this.barCodes.Rate(x.id, (byte) x.value, userId);
                                                     this.Log(x.id, userId, "RATE");
                                                 }

                                                 return Response.AsJson(new[] {this.barCodes.Get(x.id, userId)});
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