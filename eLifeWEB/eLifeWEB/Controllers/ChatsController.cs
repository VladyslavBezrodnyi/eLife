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
using System.Net;

namespace eLifeWEB.Controllers
{
    public class ChatsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chats
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            ViewBag.Role = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            IEnumerable<Conversation> conversations;
            if (ViewBag.Role == "patient")
            {
                conversations = db.Conversations.Where(e => e.PatientId == user.Id).ToList();
            }
            else
            {
                conversations = db.Conversations.Where(e => e.DoctorId == user.Id).ToList();
            }
            return View(conversations);
        }

        public ActionResult Chat(int? interlocutorId)
        {
            if (interlocutorId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            string role = db.Roles.Find(user.Roles.FirstOrDefault().RoleId).Name;
            ApplicationUser patient;
            ApplicationUser doctor;
            if (role == "patient")
            {
                patient = user;
                doctor = db.Users.FirstOrDefault(e => e.DoctorInformId == interlocutorId);
            }
            else
            {
                doctor = user;
                patient = db.Users.FirstOrDefault(e => e.PatientInformId == interlocutorId);
            }
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
            var messages = conversation
                .ConversationReplies
                .OrderBy(e => e.Time)
                .GroupBy(e => e.Time.Date)
                .Select(
                e => new ChatView()
                {
                    Date = e.Key.ToString("dd.MM.yyyy"),
                    Count = e.Count(),
                    Messages = e.Select(m => new MessageDate
                    {
                        Name = m.Sender.Name,
                        SenderId = m.SenderId,
                        Text = m.ReplyText,
                        Time = m.Time.ToString("H:mm")
                    }).ToList()
                }).ToList();
            ViewBag.Sender = (role == "patient")?(patient.Id):(doctor.Id);
            ViewBag.Conversation = conversation;
            return View(messages);
        }
    }
}
