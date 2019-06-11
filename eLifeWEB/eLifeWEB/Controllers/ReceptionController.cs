using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using eLifeWEB.Models;
using Microsoft.AspNet.Identity;
using MimeKit;
using Newtonsoft.Json;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace eLifeWEB.Controllers
{
    public class ReceptionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ReceptionPreview(int? recordId)
        {
                Record record = db.Records.Find(recordId);
                if(record.PatientId != null)
                {
                if (record.Date <= DateTime.Now && record.EndDate > DateTime.Now)
                {
                    ViewBag.Message = "Present";
                }
                else
                {
                    if (record.EndDate < DateTime.Now)
                        ViewBag.Message = "Past";
                    else
                        ViewBag.Message = "Future";
                }
            }
                else
                {
                    ViewBag.Message = "None";
                }
            
            return View(record);
        }

        public ActionResult StartReception (int? recordId)
        {
            Record record = db.Records.Find(recordId);
            ApplicationUser patient = db.Users.Find(record.PatientId);
            ViewBag.Patient = patient;
            PatientInform patientInform = db.PatientInforms.Find(patient.PatientInformId);
            ViewBag.PatientInfo = patientInform;
            var medicalCard = db.Records.Where(u => u.PatientId == record.PatientId && record.EndDate < DateTime.Now).ToList();
            ViewBag.MedicalCard = medicalCard;
            return View();
        }

        [HttpPost]
        public ActionResult StartReception(int? recordId, Record model)
        {

            Record newRecord = db.Records.Find(recordId);
            newRecord.Appointment = model.Appointment;
            newRecord.Complaints = model.Complaints;
            newRecord.CourseOfTheDisease = model.CourseOfTheDisease;
            newRecord.Diagnoses = model.Diagnoses;
            newRecord.ObjectiveData = model.ObjectiveData;
            newRecord.EndDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("MyAccount","Account");
        }



        public ActionResult CancelReception(int? recordId)
        {
            ApplicationUser mainUser = db.Users.Find(User.Identity.GetUserId());
            Record record = db.Records.Find(recordId);
            Payment payment = db.Payments.Where(p => p.RecordId == recordId &&( p.status == "sandbox" || p.status == "success")).FirstOrDefault();

            return View(LiqPayHelper.GetLiqPayRefund(payment));
        }

        [HttpPost]
        public async Task<ActionResult> CancelReception(int? recordId, int? result)
        {
                
                Record record = db.Records.Find(recordId);
                Payment payment = db.Payments.Where(p => p.RecordId == recordId && (p.status == "sandbox" || p.status == "success")).FirstOrDefault();
                // настройка логина, пароля отправителя
                var from = "elifeprojectnure@gmail.com";
                var pass = "eLifeProject";

                // создаем письмо: message.Destination - адрес получателя
                var emailMessage = new MimeMessage()
                {
                    Subject = "eLife підтвердження запису",
                    Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "<h2> " + record.Patient.Name + " , ваш лікар відмінив запис, а вам повернули гроші за прйиом на картку" + " </h2> <br>"
                        + "Лікар:" + record.AttendingDoctor.Name + " <br>" +
                        " Клініка:" + record.AttendingDoctor.DoctorInform.Clinic.Name + " < br > " +
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
                payment.status = "reversed";
                record.PatientId = null;
                db.SaveChanges();
                return RedirectToAction("MyAccount", "Account");
            }
        




    }
}