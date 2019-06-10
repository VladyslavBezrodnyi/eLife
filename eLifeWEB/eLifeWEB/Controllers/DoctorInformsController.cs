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
using Microsoft.AspNet.Identity;
using System.Text;
using Newtonsoft.Json;
using PagedList;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace eLifeWEB.Controllers.WEBControllers
{
    public class DoctorInformsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<DoctorInform> doctorInforms;
        
        // GET: DoctorInforms
        public ActionResult Index(string searchString, string specializations, int? page, bool? check)
        {
            ViewBag.Feedback = db.Feedbacks;

            var doctorInforms = db.DoctorInforms.Include(d => d.Clinic).Where(q =>q.Practiced);
            SelectList specialiation = new SelectList(new Specializations().specializations);          
            ViewBag.Specialization = specialiation;
            if (!String.IsNullOrEmpty(searchString))
            {
                doctorInforms = doctorInforms.Where(s => s.ApplicationUsers.FirstOrDefault().Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Clinic.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(specializations) && !specializations.Equals("Усі"))
            {
                if (check == true)
                {
                    doctorInforms = doctorInforms.Where(p => p.Specialization != specializations);
                }
                else
                    doctorInforms = doctorInforms.Where(p => p.Specialization == specializations);
            }
            
            SelectList types = new SelectList(new List<string>()
            {
            "Усі",
            "Акушерство та гінекологія",
            "Анестезіологія та інтенсивна терапія",
            "Дерматовенерологія",
            "Дитяча хірургія",
            "Інфекційні хвороби",
            "Медична психологія",
            "Неврологія",
            "Нейрохірургія",
            "Ортопедія і травматологія",
            "Отоларингологія",
            "Офтальмологія",
            "Патологічна анатомія",
            "Педіатрія",
            "Психіатрія",
            "Пульмонологія та фтизіатрія",
            "Урологія",
            "Хірургія"

            });
            ViewBag.Specialization = types;
            if (Request.IsAuthenticated)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.Role = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(doctorInforms.ToList().ToPagedList(pageNumber, pageSize));
           // return View(doctorInforms.ToList());
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

            var ID = doctorInform.ApplicationUsers.FirstOrDefault().Id;
            var feedbacks = db.Feedbacks.Where(u => u.DoctorId == ID).ToList();
            
            ViewBag.Feedbacks = feedbacks;
            ViewBag.Average = feedbacks.Average(u => u.Stars)*20;
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Material;
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
            scheduler.Config.first_hour = 6;
            scheduler.Config.last_hour = 20;

            scheduler.Data.Loader.AddParameter("id", id);
            scheduler.Localization.Set(SchedulerLocalization.Localizations.Ukrainian);
            scheduler.Config.drag_lightbox = true;
            scheduler.Lightbox.Clear();
            var select = new LightboxSelect("service", "Послуга");
            var services = new List<object>();
            foreach (TypeOfService type in doctorInform.ApplicationUsers.FirstOrDefault().TypeOfServices)
            {
                services.Add(new { key = type.Id, label = type.Name });
            }
            scheduler.Config.icons_select = new EventButtonList()
            {
                EventButtonList.Details
            };
            scheduler.Config.drag_create = false;
            scheduler.Config.drag_lightbox = false;
            scheduler.Config.drag_resize = false;
            scheduler.Config.drag_move = false;
            var items = services;
            select.AddOptions(items);
            scheduler.Lightbox.Add(select);
            scheduler.Config.buttons_left = new LightboxButtonList
            {
                new EventButton
                {
                    Label = "Записатися на прийом",
                    OnClick = "appointment",
                    Name = "appointment"
                },
                LightboxButtonList.Cancel,
            };
            scheduler.Config.buttons_right = new LightboxButtonList();
            if(!User.Identity.IsAuthenticated)
            {
                scheduler.Config.isReadonly = true;
            }
            ViewBag.Scheduler = scheduler;
            return View(doctorInform);
        }

        public ContentResult Data(int? id)
        {
            List<Appointment> list = new List<Appointment>();
            ApplicationDbContext db = new ApplicationDbContext();
            var records = new ApplicationDbContext().Records.Where((d => (d.TypeOfService.Doctor.DoctorInform.Id == id || d.AttendingDoctor.DoctorInform.Id == id ) && d.Patient == null));

            foreach (Record record in records)
            {
                list.Add(new Appointment{ id = record.Id, text = "Вільне місце", start_date = record.Date, end_date = record.EndDate });

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

        public ActionResult AcceptAppointment(int? id, int? serviceId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Record record = db.Records.Find(id);
            record.TypeOfServiceId = serviceId;
            TypeOfService typeOfService = db.TypeOfServices.Where(u => u.Id == serviceId).FirstOrDefault();
            ApplicationUser Patient = db.Users.Find(User.Identity.GetUserId());
            Payment payment = new Payment()
            {
                RecordId = record.Id,
                PatientId = Patient.Id,
                amount = typeOfService.Price,
                order_id = Guid.NewGuid().ToString()
            };
            ViewBag.Patient = Patient;
            ViewBag.Payment = payment;
            ViewBag.Record = record;
            db.Payments.Add(payment);
            db.SaveChanges();
            return View("Payment", LiqPayHelper.GetLiqPayModel(payment, typeOfService, Patient));

        }

        [HttpPost]
        public async Task<ActionResult> AppointmentResult()
        {
            var request_dictionary = Request.Form.AllKeys.ToDictionary(key => key, key => Request.Form[key]);

            // --- Розшифровую параметр data відповіді LiqPay та перетворюю в Dictionary<string, string> для зручності:
            byte[] request_data = Convert.FromBase64String(request_dictionary["data"]);
            string decodedString = Encoding.UTF8.GetString(request_data);
            var request_data_dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(decodedString);

            // --- Отримую сигнатуру для перевірки
            var mySignature = LiqPayHelper.GetLiqPaySignature(request_dictionary["data"]);

            // --- Якщо сигнатура серевера не співпадає з сигнатурою відповіді LiqPay - щось пішло не так
            if (mySignature != request_dictionary["signature"])
                return View("~/Views/Shared/_Error.cshtml");

            // --- Якщо статус відповіді "Тест" або "Успіх" - все добре
            if (request_data_dictionary["status"] == "sandbox" || request_data_dictionary["status"] == "success")
            {
                Payment payment = db.Payments.Find(request_data_dictionary["order_id"]);
                Record record = db.Records.Find(payment.RecordId);
                payment.status = request_data_dictionary["status"];
                record.PatientId = payment.PatientId;
                db.SaveChanges();
                // настройка логина, пароля отправителя
                var from = "elifeprojectnure@gmail.com";
                var pass = "eLifeProject";

                // создаем письмо: message.Destination - адрес получателя
                var emailMessage = new MimeMessage()
                {
                    Subject = "eLife підтвердження запису",
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "<h2> " + record.Patient.Name + " , ви успішно записались на прийом" +" </h2> <br>" 
                        + "Лікар:" + record.AttendingDoctor.Name +" <br>" +
                        " Клініка:" + record.AttendingDoctor.DoctorInform.Clinic.Name +" < br > " +
                        " Дата та час:" + record.Date + " < br > " +
                         " Вид прийому:" + record.TypeOfService.Name + " < br > "
                    }
                };
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", from));
                emailMessage.To.Add(new MailboxAddress("", record.Patient.Email));

                // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465);
                    await client.AuthenticateAsync(from, pass);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                ViewBag.Type = record.TypeOfService.Name;
                return View(record);
            }
            return View("~/Views/Shared/_Error.cshtml");
        }

      
        public async Task<ActionResult> AppointmentResultTest(int? id, int? serviceId)
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            if (id != null && user != null)
            {

                Record record = db.Records.Find(id);
                TypeOfService typeOfService = db.TypeOfServices.Find(serviceId);
                record.TypeOfServiceId = serviceId;
                record.PatientId = user.Id;
                db.SaveChanges();
                // настройка логина, пароля отправителя
                var from = "elifeprojectnure@gmail.com";
                var pass = "eLifeProject";

                // создаем письмо: message.Destination - адрес получателя
                var emailMessage = new MimeMessage()
                {
                    Subject = "eLife підтвердження запису",
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "<h2> " + record.Patient.Name + " , ви успішно записались на прийом" + " </h2> <br>"
                        + "<h3> Лікар:" + record.AttendingDoctor.Name + " </h3><br>" +
                        "< h3 > Клініка:" + record.AttendingDoctor.DoctorInform.Clinic.Name + " </ h3 >< br > " +
                        "< h3 > Дата та час:" + record.Date + " </ h3 >< br > " +
                         "< h3 > Вид прийому:" + record.TypeOfService.Name + " </ h3 >< br > "
                    }
                };
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", from));
                emailMessage.To.Add(new MailboxAddress("", record.Patient.Email));

                // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465);
                    await client.AuthenticateAsync(from, pass);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                ViewBag.Type = record.TypeOfService.Name;
                return View("AppointmentResult", record);
            }
            return View("~/Views/Shared/_Error.cshtml");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult DoctorFeedback(string DoctorID, string UserId, string Text , int Rating)
        {
            Feedback feedback = new Feedback() { DoctorId = DoctorID, Comment = Text, Stars = Rating, PatientId = UserId, Date = DateTime.Now };
            try
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
            }
            catch
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "DoctorInforms");
        }
    }
}
