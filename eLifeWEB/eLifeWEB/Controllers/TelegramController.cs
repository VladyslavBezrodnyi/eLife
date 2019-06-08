using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json;
using eLifeWEB.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using eLifeWEB.Models.TransferData;

namespace eLifeWEB.Controllers
{
    //[System.Web.Mvc.RoutePrefix("Telegram")]
    public class TelegramController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public TelegramController()
        {

        }

        public TelegramController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[System.Web.Http.HttpPost]
        //public async Task<JsonResult> Log([FromBody] LoginModel model)
        //{
        //    var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            var user = db.Users.Find(model.Email);
        //            LoginAnswer loginAnswer = new LoginAnswer("123", user.Email);
        //            string answer = JsonConvert.SerializeObject(loginAnswer);
        //            return Json(answer);
        //        default:
        //            return Json("Помилка");
        //    }
        //}
        
        [System.Web.Http.HttpPost]
        public async Task<JsonResult> Login([FromBody] LoginData model)
        {
            var result = await SignInManager.PasswordSignInAsync(model.email, model.password, false, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return Json("Ок");
                default:
                    return Json("Помилка");
            }
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRecords([FromBody] string model)
        {
            string email =  model.Replace("!", ".");
            var records = db.Records.Where(r => r.PatientId == db.Users.FirstOrDefault(u => u.Email == email).Id || r.AttendingDoctorId == db.Users.FirstOrDefault(e => e.Email == email).Id).Select(e => new RecordsData(){Time = e.Date, Service = e.TypeOfService.Name, ClinicName = e.AttendingDoctor.DoctorInform.Clinic.Name, DoctorName = e.AttendingDoctor.Name  }).ToList();

            for(int i = 0; i < records.Count;i++)
            {
                if((records[i].Time - DateTime.Now).TotalHours >= 24)
                {
                    records.Remove(records[i]);
                }
            }

            string recordsDatas = JsonConvert.SerializeObject(records);
            return Json(recordsDatas, JsonRequestBehavior.AllowGet);
        }
    }
}