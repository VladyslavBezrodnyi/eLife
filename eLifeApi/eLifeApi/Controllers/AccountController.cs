using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using eLifeApi.Models;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace eLifeApi.Controllers
{
    public class AccountController : Controller
    {
        eLifeDB db = new eLifeDB();
        public ActionResult Login()
        {
            return View();
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = db.Users.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(model);
        }
        */
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
           // if (ModelState.IsValid)
           // {
              //  User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password &&);

              //  if (user == null)
              //  {
                    db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, Role_id = 1});
                    db.SaveChanges();
                   // user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                   // if (user != null)
                   // {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                  //  }
            //    }
              //  else
                  //  ModelState.AddModelError("", "Пользователь с таким логином уже существует");
           // }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}