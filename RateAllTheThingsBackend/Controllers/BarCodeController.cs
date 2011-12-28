using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult Details(int id)
        {
            return Json(this.barCodes.Get(id), JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /BarCode/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        
        //
        // POST: /BarCode/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /BarCode/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
