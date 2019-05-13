using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using eLifeApi.Models;

namespace eLifeApi.Controllers.WEBControllers
{
    public class AccountController : Controller
    {
        private eLifeDB db = new eLifeDB();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == model.Name && u.Password == model.Password);

                if (user == null)
                {
                    db.Users.Add(new User { Email = model.Name, Password = model.Password, Role_id = 2 });
                    db.SaveChanges();
                    user = db.Users.Where(u => u.Email == model.Name && u.Password == model.Password).FirstOrDefault();
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }
            return View(model);
        }

        public ActionResult ChoiceRoleRegister(User user)
        {
            return View();
        }

        public ActionResult ChoiceRoleRegister(int? role, User user)
        {
            if (role != null)
            {
                if (user != null)
                {
                    user.Role_id = (int)role;
                    user.Role = db.Roles.Find(role);
                    user.Role.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
           }
                else { 
                    ModelState.AddModelError("", "Выберите роль");
            }
            return View();
        }
        

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
