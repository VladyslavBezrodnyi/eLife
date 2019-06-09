using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLifeWEB.Models;

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


    }
}