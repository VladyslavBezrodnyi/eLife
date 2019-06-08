using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using eLifeWEB.Models;
using eLifeWEB.Utils;
using System.IO;
using System.Data.Entity;
using DHTMLX.Scheduler;
using System.Collections.Generic;
using DHTMLX.Scheduler.Data;
using static eLifeWEB.Controllers.ManageController;
using DHTMLX.Common;

namespace eLifeWEB.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public ActionResult MyAccount(ManageMessageId? message)
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.Role = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            var scheduler = new DHXScheduler(this);
            
            scheduler.Skin = DHXScheduler.Skins.Material;
            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;
            scheduler.Config.show_loading = true;
            scheduler.Config.first_hour = 6;
            scheduler.Config.last_hour = 20;
            scheduler.Data.Loader.AddParameter("id", user.Id);
            scheduler.Localization.Set(SchedulerLocalization.Localizations.Ukrainian);
            scheduler.Config.drag_lightbox = true;
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Readonly);
            ViewBag.Scheduler = scheduler;
            return View(user);
        }

        public ContentResult Data(string id)
        {
            List<Appointment> list = new List<Appointment>();
            ApplicationDbContext db = new ApplicationDbContext();
            var records = new ApplicationDbContext().Records.Where((d => d.TypeOfService.Doctor.Id == id || d.AttendingDoctorId == id)).ToList();

            foreach (Record record in records)
            {
                if(record.PatientId == null)
                    list.Add(new Appointment{ id = record.Id, text = "Вільне місце", start_date = record.Date, end_date = record.EndDate, @readonly = false });
                else
                {
                    list.Add(new Appointment { id = record.Id, text = "Запис" + "\n"+ "Пацієнт: " + record.Patient.Name +"\n" + record.TypeOfService.Name, start_date = record.Date, end_date = record.EndDate, @readonly = true });
                }
            }
            return new SchedulerAjaxData(list);

        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            try
            {
                var changedEvent = DHXEventsHelper.Bind<Appointment>(actionValues);
                Record record = db.Records.Find(id);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

                        record = new Record() {
                        Date = changedEvent.start_date,
                        EndDate = changedEvent.end_date,
                        AttendingDoctorId = user.Id
                        
                        };
                        db.Records.Add(record);
                        
                        break;
                    case DataActionTypes.Delete:
                        db.Entry(record).State = EntityState.Deleted;
                        break;
                    default:// "update" 
                        record = db.Records.Find(id);
                        record.Date = changedEvent.start_date;
                        record.EndDate = changedEvent.end_date;
                        break;
                }
                db.SaveChanges();
                action.TargetId = changedEvent.id;
        }
            catch (Exception a)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
        }

        public ActionResult MedicalCard(string id)
        {
            var records = db.Records.Where(r => r.PatientId == id);
            return View(records.ToList());
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Требовать предварительный вход пользователя с помощью имени пользователя и пароля или внешнего имени входа
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Приведенный ниже код защищает от атак методом подбора, направленных на двухфакторные коды. 
            // Если пользователь введет неправильные коды за указанное время, его учетная запись 
            // будет заблокирована на заданный период. 
            // Параметры блокирования учетных записей можно настроить в IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неправильный код.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // генерируем токен для подтверждения регистрации
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // создаем ссылку для подтверждения
                    var callbackUrl = Url.Action
                        (
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: Request.Url.Scheme
                        );
                    // отправка письма
                    await UserManager.SendEmailAsync(
                        user.Id,
                        "Подтверждение электронной почты",
                        "Для завершения регистрации перейдите по ссылке:: <a href=\"" + callbackUrl + "\">завершить регистрацию</a>");
                    return View("DisplayEmail");
                    //return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }
        [Authorize]
        public ActionResult ChoiceRoleRegister()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ChoiceRoleRegister(string role)
        {
            if (!String.IsNullOrEmpty(role))
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                if (user != null)
                {
                    UserManager.AddToRole(user.Id, role);
                    db.SaveChanges();
                    if (role == "patient")
                        return RedirectToAction("RegisterPatient", "Account");
                    if (role == "doctor")
                        return RedirectToAction("RegisterDoctor", "Account");
                    if (role == "clinicAdmin")
                        return RedirectToAction("RegisterClinicAdmin", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Выберите роль");
            }
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            ApplicationUser user = UserManager.FindById(userId);
            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Не показывать, что пользователь не существует или не подтвержден
                    return View("ForgotPasswordConfirmation");
                }

                // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
                // Отправка сообщения электронной почты с этой ссылкой
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Сброс пароля", "Сбросьте ваш пароль, щелкнув <a href=\"" + callbackUrl + "\">здесь</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // Появление этого сообщения означает наличие ошибки; повторное отображение формы
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Не показывать, что пользователь не существует
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Запрос перенаправления к внешнему поставщику входа
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Создание и отправка маркера
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Выполнение входа пользователя посредством данного внешнего поставщика входа, если у пользователя уже есть имя входа
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // Если у пользователя нет учетной записи, то ему предлагается создать ее
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Получение сведений о пользователе от внешнего поставщика входа
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        //return RedirectToLocal(returnUrl);
                        return RedirectToAction("ChoiceRoleRegister", "Account");
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        [Authorize]
        public ActionResult RegisterPatient()
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
            string[] genders = new[]
            {
                "Жіночий",
                "Чоловічий"
            };
            ViewBag.Genders = new SelectList(genders);
            ViewBag.Bloodgroup = new SelectList(bloodgroups);
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult RegisterPatient(RegisterPatientModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId()); ;
                PatientInform patientInform = new PatientInform { Allergy = model.Allergy, BloodGroup = model.BloodGroup, Diabetes = model.Diabetes, Activity = model.Activity, Adress = model.Adress, Infectious_diseases = model.Infectious_diseases, Operations = model.Operations };
                db.PatientInforms.Add(patientInform);
                db.SaveChanges();
                user.PatientInformId = patientInform.Id;
                user.Name = model.Name;
                user.Gender = model.Gender;
                user.Bithday = model.Birthday;
                
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        
        public ActionResult RegisterDoctor()
        {
            SelectList specialiation = new SelectList(new Specializations().specializations);
            ViewBag.Specialization = specialiation;
            string[] genders = new[]
            {
                "Жіноча",
                "Чоловіча"
            };
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name");

            ViewBag.Genders = new SelectList(genders);
            return View();
        }
        
        [HttpPost]
        public ActionResult RegisterDoctor(RegisterDoctorModel model, HttpPostedFileBase uploadImage)
        {
            ViewBag.ClinicId = new SelectList(db.Clinics, "Id", "Name");
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId()); 
                    DoctorInform doctorInform = new DoctorInform
                    {
                        Category = model.Category,
                        Education = model.Education,
                        Guardian = model.Guardian,
                        Practiced = false,
                        Specialization = model.Specialization,
                        ClinicId = model.СlinicId,
                        Skills = model.Skills,
                        Image = imageData
                    };
                    db.DoctorInforms.Add(doctorInform);
                    db.SaveChanges();
                    user.DoctorInformId = doctorInform.Id;
                    user.DoctorInform = doctorInform;
                    user.Name = model.Name;
                    user.Gender = model.Gender;
                    user.Bithday = model.Birthday;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Оберіть зображення");
                }
            }
            return View(model);
        }

        public ActionResult EditGeneralInfoPatient()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
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
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                PatientInform newPatient = db.PatientInforms.Find(user.PatientInform.Id);
                newPatient.Activity = patientInform.Activity;
                newPatient.Adress = patientInform.Adress;
                newPatient.Allergy = patientInform.Allergy;                
                newPatient.BloodGroup = patientInform.BloodGroup;
                newPatient.Diabetes = patientInform.Diabetes;
                newPatient.Infectious_diseases = patientInform.Infectious_diseases;
                newPatient.Operations = patientInform.Operations;
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(patientInform);
        }

        public ActionResult EditGeneralInfoDoctor()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            SelectList clinics = new SelectList(db.Clinics, "Id", "Name");
            ViewBag.Clinics = clinics;
            return View(user.DoctorInform);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGeneralInfoDoctor(DoctorInform doctorInform, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                DoctorInform newDoctor = db.DoctorInforms.Find(user.DoctorInformId);
                if (uploadImage != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    newDoctor.Image = imageData;
                }
                newDoctor.ClinicId = doctorInform.ClinicId;
                newDoctor.Practiced = doctorInform.Practiced;
                newDoctor.Skills = doctorInform.Skills;
                newDoctor.Specialization = doctorInform.Specialization;
                newDoctor.Education = doctorInform.Education;
                newDoctor.Category = doctorInform.Category;
                newDoctor.Guardian = doctorInform.Guardian;
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(doctorInform);
        }

        public ActionResult EditClinic()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            return View(user.ClinicAdmin.Clinic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditClinic([Bind(Include = "Id,Name,Adress,Description,BankCard")] Clinic clinic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                Clinic newClinic = db.Clinics.Find(clinic.Id);
                newClinic.Name = clinic.Name;
                newClinic.Description = clinic.Description;
                newClinic.Adress = clinic.Adress;
                newClinic.BankCard = clinic.BankCard;
                newClinic.Image = clinic.Image;
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    newClinic.Image = imageData;
                }
                db.SaveChanges();
                return RedirectToAction("MyAccount");
            }
            return View(clinic);
        }

        
        public ActionResult RegisterClinicAdmin()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult RegisterClinicAdmin([Bind(Include = "Id_clinic,Name,Adress,Description,BankCard,Image")] Clinic clinic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage == null)
                {
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId()); ;
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
                    ClinicAdmin clinicAdmin = new ClinicAdmin()
                    {
                        ClinicId = clinic.Id,
                        ClinicConfirmed = true,
                    };
                    db.ClinicAdmins.Add(clinicAdmin);
                    db.SaveChanges();
                    user.ClinicAdminId = clinicAdmin.Id;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Выберите изображение");
                }
            }
            return View(clinic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Вспомогательные приложения
        // Используется для защиты от XSRF-атак при добавлении внешних имен входа
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}