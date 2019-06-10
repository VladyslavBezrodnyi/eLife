using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eLifeWEB.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace eLifeWEB.Controllers.WEBControllers
{
    public class ClinicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clinics
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(db.Clinics.ToList().ToPagedList(pageNumber, pageSize));
            //return View(db.Clinics.ToList());
        }

        // GET: Clinics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // GET: Clinics/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinics/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_clinic,Name,Adress,Description,BankCard,Image")] Clinic clinic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                }
                clinic.Image = imageData;
                db.Clinics.Add(clinic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clinic);
        }

        // GET: Clinics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return HttpNotFound();
            }
            return View(clinic);
        }

        // POST: Clinics/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_clinic,Name,Adress,Description,BankCard")] Clinic clinic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                }
                clinic.Image = imageData;
                db.Entry(clinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clinic);
        }
        public ActionResult ConfirmationDoctors()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            var doctors = user.ClinicAdmin.Clinic.DoctorInforms.OrderBy(e => e.ApplicationUsers.FirstOrDefault().Name);
            return View(doctors);
        }
        [HttpPost]
        public ActionResult ConfirmationDoctors(FormCollection collectionPractice)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (collectionPractice.Count != 0)
            {
                foreach (var item in user.ClinicAdmin.Clinic.DoctorInforms)
                {
                    Boolean tempValue = collectionPractice[item.Id.ToString()] != null ? true : false;
                    if (tempValue == true)
                    {
                        item.Practiced = true;
                    }
                    else
                    {
                        item.Practiced = false;
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("MyAccount", "Account");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
