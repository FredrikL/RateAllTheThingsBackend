using System;
using System.Web.Mvc;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controllers
{
    public class CommentController : Controller
    {
        private readonly IComments comments;

        public CommentController()
        {
            this.comments = TinyIoC.TinyIoCContainer.Current.Resolve<IComments>();
        }

        [HttpPost]
        public ActionResult Add(Int64 barCodeId, Comment comment)
        {
            return Json(this.comments.Add(barCodeId, comment));
        }

        [HttpGet]
        public ActionResult GetForComment(Int64 barCodeId)
        {
            return Json(this.comments.GetCommentsForBarCode(barCodeId), JsonRequestBehavior.AllowGet);
        }
    }
}
