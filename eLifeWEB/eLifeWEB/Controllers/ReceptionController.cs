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
                if(record.PatientId == null)
                {
                    if(record.Date <= DateTime.Now &&record.EndDate > DateTime.Now)
                        ViewBag.Message = "Present";
                    if (record.EndDate > DateTime.Now)
                        ViewBag.Message = "Past";
                    ViewBag.Message = "Future";
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
            ViewBag.Patient = record.Patient;
            var medicalCard = db.Records.Where(u => u.PatientId == record.PatientId && record.EndDate < DateTime.Now).ToList();
            ViewBag.MedicalCard = medicalCard;
            return View();
        }
    }
}