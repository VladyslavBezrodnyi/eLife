using eLifeWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLifeWEB.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Admin()
        {
            var clinics = db.ClinicAdmins.ToList();
            ViewBag.Password = db.Users.Find("admin").PasswordHash;
            return View(clinics);
        }

        [HttpPost]
        public ActionResult Admin(FormCollection collection)
        {
            var clinics = db.ClinicAdmins.ToList();
            if (collection != null)
            {
                foreach (var item in clinics)
                {
                    if (!string.IsNullOrEmpty(collection[item.Id.ToString()]))
                    {
                        if (Convert.ToBoolean(collection[item.Id.ToString()].Split(',')[0]))
                        {
                            item.ClinicConfirmed = true;
                        }
                        else
                        {
                            item.ClinicConfirmed = false;
                        }
                    }

                }
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}