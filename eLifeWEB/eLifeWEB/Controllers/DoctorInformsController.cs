using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eLifeWEB.Utils;
using eLifeWEB.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using DHTMLX.Scheduler.Controls;

namespace eLifeWEB.Controllers.WEBControllers
{
    public class DoctorInformsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int? doctorId = null;

        // GET: DoctorInforms
        public ActionResult Index(string searchString, string specializations)
        {
            var doctorInforms = db.DoctorInforms.Include(d => d.Clinic).Where(q =>q.Practiced);
            SelectList specialiation = new SelectList(new Specializations().specializations);
            ViewBag.Specialization = specialiation;
            if (!String.IsNullOrEmpty(searchString))
            {
                doctorInforms = doctorInforms.Where(s => s.ApplicationUsers.FirstOrDefault().Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Clinic.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(specializations))
            {
                doctorInforms = doctorInforms.Where(s => s.Specialization == specializations);
            }
            return View(doctorInforms.ToList());
        }

        // GET: DoctorInforms/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorInform doctorInform = db.DoctorInforms.Find(id);
            if (doctorInform == null)
            {
                return HttpNotFound();
            }
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Material;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.Config.first_hour = 6;
            sched.Config.last_hour = 20;
            sched.Data.Loader.AddParameter("id", id);
            sched.Localization.Set(SchedulerLocalization.Localizations.Ukrainian);
            sched.Config.drag_lightbox = true;
            sched.Lightbox.Clear();
            var select = new LightboxSelect("service", "Послуга");
            //var items = new List<object>();
            var services = new List<object>();
            foreach (TypeOfService type in doctorInform.ApplicationUsers.FirstOrDefault().TypeOfServices)
            {
                services.Add(new { key = type.Id, label = type.Type.Type1 });
            }
            var items = services;
            select.AddOptions(items);
            sched.Lightbox.Add(select);
            EventButton button = LightboxButtonList.Save;
            button.Label = "Записатися на прийом"; 
            sched.Config.buttons_left = new LightboxButtonList
            {
                //new EventButton
                //{
                //    Label = "Записатися на прийом",
                //    OnClick = "lightbox.save()",
                //    Name = "appointment"
                //},
                button, 
                LightboxButtonList.Cancel,
            };
            sched.Config.buttons_right = new LightboxButtonList();
            ViewBag.Scheduler = sched;
            return View(doctorInform);
        }

        public ContentResult Data(int? id)
        {
            List<object> list = new List<object>();
            ApplicationDbContext db = new ApplicationDbContext();
            var records = new ApplicationDbContext().Records.Where((d => d.TypeOfService.Doctor.DoctorInform.Id == id && d.Patient == null));

            foreach (Record record in records)
            {
                list.Add(new { id = record.Id, text = "Вільне місце", start_date = record.Date, end_date = record.Date.AddHours(2) });

            }
            return new SchedulerAjaxData(list);

        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                return (new AjaxSaveResponse(action));
            }
            finally
            {

                RedirectToAction("Index");
            }
        }

        private ActionResult RedirectAfterSaving()
        {
            return RedirectToAction("Index", new { target = "_blank"});
        }
        

        // GET: DoctorInforms/Create
        public ActionResult Create()
        {
            ViewBag.Id_clinic = new SelectList(db.Clinics, "Id_clinic", "Name");
            return View();
        }

        // POST: DoctorInforms/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Specialization,Category_,Guardian,Id_clinic,Education,Skills,Practiced,Photo")] DoctorInform doctorInform)
        {
            if (ModelState.IsValid)
            {
                db.DoctorInforms.Add(doctorInform);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_clinic = new SelectList(db.Clinics, "Id_clinic", "Name", doctorInform.ClinicId);
            return View(doctorInform);
        }

        // GET: DoctorInforms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DoctorInform doctorInform = db.DoctorInforms.Find(id);
            if (doctorInform == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_clinic = new SelectList(db.Clinics, "Id_clinic", "Name", doctorInform.ClinicId);
            return View(doctorInform);
        }

        // POST: DoctorInforms/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Specialization,Category_,Guardian,Id_clinic,Education,Skills,Practiced,Photo")] DoctorInform doctorInform)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorInform).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_clinic = new SelectList(db.Clinics, "Id_clinic", "Name", doctorInform.ClinicId);
            return View(doctorInform);
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
