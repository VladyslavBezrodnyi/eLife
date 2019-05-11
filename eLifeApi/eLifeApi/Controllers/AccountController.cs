using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using eLifeApi.Models;
using System.Net.Http;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;


using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace eLifeApi.Controllers
{
    public class AccountController : ApiController
    {
        eLifeDB db = new eLifeDB();
        //public ActionResult Login()
        //{
        //    return View();
        //}
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IHttpActionResult Login(LoginModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    // поиск пользователя в бд
               //User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

            //    if (user != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(model.Name, true);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
            //    }
            //}

            if (ModelState.IsValid)
            {
                User user = db.Users.Find(model.Email, model.Password);
                if(user != null)
                {
                    return Json(user);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

            
        }
        
        //public ActionResult Register()
        //{
        //    //return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseType(typeof(void))]
        public IHttpActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid) {
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email );

                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, Role_id = 1 });
                    db.SaveChanges();
                    return Json(user);
                }
                else
                {
                   return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
            //return View(model);
            
        }
        //public ActionResult Logoff()
        //{
        //    FormsAuthentication.SignOut();
        //    //return RedirectToAction("Index", "Home");
        //}
    }
}