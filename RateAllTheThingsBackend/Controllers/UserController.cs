using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RateAllTheThingsBackend.Repositories;

namespace RateAllTheThingsBackend.Controllers
{
    public class UserController : Controller
    {
        private IUsers users;

        public UserController()
        {
            this.users = TinyIoC.TinyIoCContainer.Current.Resolve<IUsers>();
        }

        [HttpPost]
        public ActionResult Create(string email)
        {
            var fromDb = this.users.Get(email);
            if (fromDb != null)
                return Json(fromDb);
            
            var password = this.users.Create(email);
            return Json(password);           
        }

    }
}