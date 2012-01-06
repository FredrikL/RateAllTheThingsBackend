using System;
using System.Web.Mvc;
using RateAllTheThingsBackend.Core;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controllers
{
    public class BarCodeController : Controller
    {
        private readonly IBarCodes barCodes;

        public BarCodeController()
        {
            this.barCodes = TinyIoC.TinyIoCContainer.Current.Resolve<IBarCodes>();
        }

        public ActionResult Index()
        {
            return Json(this.barCodes.All(), JsonRequestBehavior.AllowGet);
        }

        [CustomBasicAuthorize]
        public ActionResult Details(string format, string code)
        {
            var item = this.barCodes.Get(format, code) ?? this.barCodes.Create(format, code);

            return Json( item ,JsonRequestBehavior.AllowGet);
        }       
        
        [HttpPost]
        [CustomBasicAuthorize]
        public ActionResult Edit(BarCode barCode)
        {
            var originalCode = this.barCodes.Get(barCode.Id);
            if(originalCode.Format == barCode.Format && originalCode.Code == barCode.Code)
            {
                originalCode.Name = barCode.Name;

                barCode = this.barCodes.Update(originalCode);
            }
            return Json(barCode);
        }

        [HttpPost]
        [CustomBasicAuthorize]
        public ActionResult Rate(Int64 id, int rating)
        {
            // Rate

            return Json(this.barCodes.Get(id));
        }
    }
}
