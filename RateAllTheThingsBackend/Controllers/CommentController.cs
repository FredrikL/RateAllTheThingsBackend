using System.Web.Mvc;
using RateAllTheThingsBackend.Models;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controllers
{
    public class CommentController : Controller
    {
        private IComments comments;

        public CommentController()
        {
            this.comments = TinyIoC.TinyIoCContainer.Current.Resolve<IComments>();
        }

        [HttpPost]
        public ActionResult Add(int barCodeId, Comment comment)
        {
            return Json(this.comments.Add(barCodeId, comment));
        }

    }
}
