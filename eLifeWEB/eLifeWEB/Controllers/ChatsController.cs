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
using System.Collections.Generic;

namespace eLifeWEB.Controllers
{
    public class ChatsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chats
        public ActionResult Index()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            string role = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            var conversations = db.Conversations.Where(e => e.DoctorId == user.Id || e.PatientId == user.Id).OrderBy(e => e.Date);
            return View(conversations);
        }

        public ActionResult Chat(int? doctorId)
        {
            ApplicationUser patient = db.Users.Find(User.Identity.GetUserId());
            ApplicationUser doctor = db.Users.FirstOrDefault(e => e.DoctorInformId == doctorId);
            var conversation = db.Conversations.FirstOrDefault(e => e.DoctorId == doctor.Id && e.PatientId == patient.Id);
            if (conversation == null)
            {
                conversation = new Conversation
                {
                    DoctorId = doctor.Id,
                    PatientId = patient.Id,
                    Date = DateTime.Now
                };
                db.Conversations.Add(conversation);
                db.SaveChanges();
            }
            ViewBag.Messeges = conversation.ConversationReplies.OrderBy(e => e.Time);
            ViewBag.Sender = patient.Id;
            return View(conversation);
        }

        // GET: Chats/Details/5
        public ActionResult Details(string DoctorId, string PatientId)
        {
            var conversation = db.ConversationReplies.Where(e => e.SenderId == DoctorId || e.SenderId == PatientId).OrderBy(e => e.Time);
            
            return View(conversation);
        }

        // GET: Chats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chats/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Chats/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Chats/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Chats/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Chats/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
