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

using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

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
        public async  Task<ActionResult> Register(RegisterModel model)
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
                        // генерация токена для пользователя
                       // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user.ToString());
                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "Account",
                             new { userId = user.Id, code = user.Id},
                             protocol: Request.Url.Scheme);
                        EmailService emailService = new EmailService();
                        await emailService.SendEmailAsync(model.Email, "Confirm your account",
                            $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
                        FormsAuthentication.SetAuthCookie(user.Email, true);
                        return RedirectToAction("ChoiceRoleRegister", "Account", new { id = user.Id });
                    }
                }
                else
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }
            return View(model);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await db.Users.FindAsync(Convert.ToInt32(userId));
            if (user == null)
            {
                return View("Error");
            }
            if (userId == code)
            {
                user.EmailConfimed = true;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            } else
            {
                return View("Error");
            }
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
            string[] bloodgroups = new[] {
                "0+ (Перша резус позитивний)",
                "0- (Перша резус негативний)",
                "A- (Друга резус негативний)",
                "A+ (Друга резус позитивний)",
                "B- (Третя резус негативний)",
                "B+ (Третя резус позитивний)",
                "AB- (Четверта резус негативний)",
                "AB+ (Четверта резус позитивний)"
            };
            ViewBag.Bloodgroup = new SelectList(bloodgroups);
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
            SelectList specialiation = new SelectList(new Specializations().specializations);
            ViewBag.Specialization = specialiation;
            return View();
        }

        [HttpPost]
        public ActionResult RegisterDoctor(RegisterDoctorModel model, HttpPostedFileBase uploadImage, int id)
        {
            if (ModelState.IsValid )
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
             
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
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
            if (String.Compare(user.Role.Name, "Patient") == 0)
                return RedirectToAction("AccountPatient");
            if (String.Compare(user.Role.Name, "Doctor") == 0)
                return RedirectToAction("AccountDoctor");
            return RedirectToAction("Login"); ;
        }
        [Authorize]
        public ActionResult AccountDoctor()
        {
            User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(user);
        }

        public ActionResult EditGeneralInfoDoctor(int id)
        {
            User user = db.Users.Find(id);
            SelectList clinics = new SelectList(db.Clinics, "Id_clinic", "Name");
            ViewBag.Clinics = clinics;
            return View(user.DoctorInform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGeneralInfoDoctor(DoctorInform doctorInform, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.Find(doctorInform.Id);
                DoctorInform newDoctor = db.DoctorInforms.Find(user.DoctorId);
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    newDoctor.Photo = imageData;
                }
                newDoctor.Id_clinic = doctorInform.Id_clinic;
                newDoctor.Practiced = doctorInform.Practiced;
                newDoctor.Skills = doctorInform.Skills;
                newDoctor.Specialization = doctorInform.Specialization;
                newDoctor.Education = doctorInform.Education;
                newDoctor.Category_ = doctorInform.Category_;
                newDoctor.Guardian = doctorInform.Guardian;
                db.SaveChanges();
                return RedirectToAction("AccountDoctor");
            }
            return View(doctorInform);
        }

        public ActionResult EditGeneralInfoPatient(int id)
        {
            User user = db.Users.Find(id);
            string[] bloodgroups = new[] {
                "0+ (Перша резус позитивний)",
                "0- (Перша резус негативний)",
                "A- (Друга резус негативний)",
                "A+ (Друга резус позитивний)",
                "B- (Третя резус негативний)",
                "B+ (Третя резус позитивний)",
                "AB- (Четверта резус негативний)",
                "AB+ (Четверта резус позитивний)"
            };
            ViewBag.Bloodgroups = new SelectList(bloodgroups);
            return View(user.PatientInform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGeneralInfoPatient(PatientInform patientInform)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
                PatientInform newPatient = db.PatientInforms.Find(user.PatientInform.PatientInfoId);
                newPatient.Activity = patientInform.Activity;
                newPatient.Adress = patientInform.Adress;
                newPatient.Allergy = patientInform.Allergy;
                newPatient.BankCard = patientInform.BankCard;
                newPatient.BloodGroup = patientInform.BloodGroup;
                newPatient.Diabetes = patientInform.Diabetes;
                newPatient.Infectious_diseases = patientInform.Infectious_diseases;
                newPatient.Operations = patientInform.Operations;
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(patientInform);
        }

        public ActionResult EditUserInfoDoctor(int id)
        {
            User user = db.Users.Find(id);
            string[] genders = new[] { "Жіноча", "Чоловіча" };
            ViewBag.Genders = new SelectList(genders);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfoDoctor(User user)
        {
            if (ModelState.IsValid)
            {
                User newUser = db.Users.Find(user.Id);
                newUser.Name = user.Name;
                newUser.Gender = user.Gender;
                newUser.Bithday = newUser.Bithday;
                newUser.Email = user.Email;
                newUser.PhoneNumber = user.PhoneNumber;
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(user);
        }

        public ActionResult ChangePassword(int id)
        {
            ChangePasswordModel model = new ChangePasswordModel();
            User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            model.OldPassword = user.Password; 
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);

                if (String.Compare(user.Password, model.OldPassword) == 0)
                {
                    user.Password = model.Password;
                    db.SaveChanges();
                    return RedirectToAction("MyAccount", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Пароль не співпадає з попереднім");
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult AccountPatient()
        {
            User user = db.Users.FirstOrDefault(u => u.Email == HttpContext.User.Identity.Name);
            return View(user);
        }

        public ActionResult Google()
        {
            GoogleAuth auth = new GoogleAuth();
            auth.CreateGoooleClient();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
