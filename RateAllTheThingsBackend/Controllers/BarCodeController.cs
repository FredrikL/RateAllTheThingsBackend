using System.Web.Mvc;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controllers
{
    public class BarCodeController : Controller
    {
        private IBarCodes barCodes;

        public BarCodeController()
        {
            this.barCodes = TinyIoC.TinyIoCContainer.Current.Resolve<IBarCodes>();
        }

        public ActionResult Index()
        {
            return Json(this.barCodes.All(), JsonRequestBehavior.AllowGet);
        }

        // return item, creates new if unknown
        public ActionResult Details(string format, string code)
        {
            var item = this.barCodes.Get(format, code);
            return Json(
                item ?? this.barCodes.Create(format, code)
                ,JsonRequestBehavior.AllowGet);
        }       
        
        [HttpPost]
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
    }
}
