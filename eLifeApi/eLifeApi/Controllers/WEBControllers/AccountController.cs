using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using eLifeApi.Models;
using Newtonsoft.Json;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

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
                    FormsAuthentication.SetAuthCookie(user.Name, true);
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
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    db.Users.Add(new User { Email = model.Email, Password = model.Password, Name = model.Name });
                    db.SaveChanges();
                    user = db.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, true);
                        return RedirectToAction("ChoiceRoleRegister", "Account", new { id = user.Id });
                    }
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }
            return View(model);
        }

        public ActionResult ChoiceRoleRegister(int id)
        {

            return View(db.Roles.ToList());
        }
        [HttpPost]
        public ActionResult ChoiceRoleRegister(int? role, int id)
        {
            if (role != null)
            {
                User user = db.Users.Find(id);
                if (user != null)
                {
                    User newUser = db.Users.Find(user.Id);
                    newUser.Role_id = (int)role;
                    //user.Role = db.Roles.Find(role);
                    //user.Role.Users.Add(user);
                    db.SaveChanges();
                    if(role == 1)
                        return RedirectToAction("RegisterPatient", "Account", new { id = user.Id });
                    if(role == 2)
                        return RedirectToAction("RegisterDoctor", "Account", new { id = user.Id });
                }
           }
                else { 
                    ModelState.AddModelError("", "Выберите роль");
            }
            return View();
        }

        public ActionResult RegisterPatient(int id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegisterPatient(RegisterPatientModel model, int id)
        {
            if (ModelState.IsValid)
            {
                    User user = db.Users.Find(id);
                    PatientInform patientInform = new PatientInform { Allergy = model.Allergy, BloodGroup = model.BloodGroup, Diabetes = model.Diabetes, Activity = model.Activity, Adress = model.Adress, Infectious_diseases = model.Infectious_diseases, BankCard = model.BankCard, Operations = model.Operations };
                    db.PatientInforms.Add(patientInform);
                    db.SaveChanges();
                    user.PatientId = patientInform.PatientInfoId;
                    db.SaveChanges();
                    return RedirectToAction("MyAccount", "Account");
            }
            return View(model);
        }

        public ActionResult RegisterDoctor(int id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegisterDoctor(RegisterDoctorModel model, HttpPostedFileBase uploadImage, int id)
        {
            if (ModelState.IsValid )
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                     imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                User user = db.Users.Find(id);
                DoctorInform doctorInform = new DoctorInform {
                    Category_ = model.Category_,
                    Education = model.Education,
                    Guardian = model.Guardian,
                    Practiced = true,
                    Specialization = model.Specialization,
                    Id_clinic = model.Id_clinic,
                    Skills = model.Skills,
                    Photo = imageData
                };
                db.DoctorInforms.Add(doctorInform);
                db.SaveChanges();
                user.DoctorId = doctorInform.Id;
                db.SaveChanges();
                return RedirectToAction("MyAccount", "Account");
            }
            return View(model);
        }

        [Authorize]
        public ActionResult MyAccount()
        {
           
            User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(user);
        }

        public ActionResult AccountDoctor()
        {
            User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(user.DoctorInform);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
