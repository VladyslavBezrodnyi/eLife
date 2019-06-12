using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eLifeApi.Models;
using System.Net.Http;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Data.Entity;


using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace eLifeApi.Controllers
{
    public class AccountController : ApiController
    {
        eLifeDB db = new eLifeDB();
        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {

            //LoginModel model = JsonConvert.DeserializeObject<LoginModel>(value);

            if (model == null)
            {
                return BadRequest();
            }
            else
            {
                User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == model.Email);
              
                Person person = new Person(user.Id, user.Name, user.Email);
                return Json(user);
            }


        }

        //[HttpPost]
        //public IHttpActionResult Register([FromBody] RegisterModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        User user = db.Users.FirstOrDefault(u => u.Email == model.Email);

        //        if (user == null)
        //        {
        //            db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, Role_id = 1 });
        //            db.SaveChanges();
        //            return Json(user);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //    //return View(model);

        //}
    }
}