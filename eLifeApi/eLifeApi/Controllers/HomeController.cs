using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace eLifeApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Name = User.Identity.Name;
            }

            return View();
        }

    }
}
