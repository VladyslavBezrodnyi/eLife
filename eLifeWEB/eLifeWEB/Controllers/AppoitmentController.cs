using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;
using eLifeWEB.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLifeWEB.Controllers
{
    public class AppoitmentController : Controller
    {
        // GET: Appoitment
        public ActionResult Index()
        {
            var sched = new DHXScheduler(this);
            sched.Skin = DHXScheduler.Skins.Material;
            sched.LoadData = true;
            sched.EnableDataprocessor = true;
            sched.Config.first_hour = 6;
            sched.Config.last_hour = 20;
            //sched.Config.drag_lightbox = true;
            //sched.Lightbox.Clear();
            //var select = new LightboxSelect("specialization", "Спеціальність");
            //var items = new List<object>(){
            //new { key = "gray", label = "Low" },
            //new { key = "blue", label = "Medium" },
            //new { key = "red", label = "High" }
            //};
            //select.AddOptions(items);
            //sched.Lightbox.Add(select);
            //sched.Config.buttons_left = new LightboxButtonList
            //{
            //    new EventButton
            //    {
            //        Label = "Записатися на прийом",
            //        OnClick = "some_function",
            //        Name = "appointment"
            //    }, 
            //    LightboxButtonList.Cancel,
            //};
            sched.Localization.Set(SchedulerLocalization.Localizations.Ukrainian);
            return View(sched);
        }

        public ContentResult Data()
        {
            var data = new SchedulerAjaxData();
            List<object> list = new List<object>();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            var records = new ApplicationDbContext().Records.Where(d => d.TypeOfService.Doctor.Id == user.Id);
           
            foreach (Record record in records)
            {
                list.Add(new { id = record.Id, text = "Вільне місце", start_date = record.Date, end_date = record.Date.AddHours(2) });
               
            }
            data.Add(list);
            return new SchedulerAjaxData(list);

        }

        public ContentResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            //var changedEvent = DHXEventsHelper.Bind<Event>(actionValues);
            //var entities = new SchedulerContext();
            //try
            //{
            //    switch (action.Type)
            //    {
            //        case DataActionTypes.Insert:
            //            entities.Events.Add(changedEvent);
            //            break;
            //        case DataActionTypes.Delete:
            //            changedEvent = entities.Events.FirstOrDefault(ev => ev.id == action.SourceId);
            //            entities.Events.Remove(changedEvent);
            //            break;
            //        default:// "update"
            //            var target = entities.Events.Single(e => e.id == changedEvent.id);
            //            DHXEventsHelper.Update(target, changedEvent, new List<string> { "id" });
            //            break;
            //    }
            //    entities.SaveChanges();
            //    action.TargetId = changedEvent.id;
            //}
            //catch (Exception a)
            //{
            //    action.Type = DataActionTypes.Error;
            //}

            return (new AjaxSaveResponse(action));
        }
    }
}