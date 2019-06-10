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
            ViewBag.Password = "123456";
            return View(clinics);
        }

        [HttpPost]
        public ActionResult Admin(List<int> clinicConfirm)
        {
            var clinics = db.ClinicAdmins.ToList();
            ViewBag.Password = "123456";
            if (clinicConfirm != null)
            {
                foreach (var item in clinics)
                {
                    if (clinicConfirm.Contains(item.Id))
                    {
                        item.ClinicConfirmed = true;
                    }
                    else
                    {
                        item.ClinicConfirmed = false;
                    }
                }
                db.SaveChanges();
            }
            else
            {
                foreach (var item in clinics)
                {
                    item.ClinicConfirmed = false;
                }
                db.SaveChanges();
            };
            return RedirectToAction("Index", "Home");
        }
    }
}