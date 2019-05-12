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


using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace eLifeApi.Controllers
{
    public class RegisterController : ApiController
    {
        eLifeDB db = new eLifeDB();

        // GET: api/Register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Register/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Register
        [HttpPost]
        public IHttpActionResult Registration([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password, Role_id = 1 });
                    db.SaveChanges();
                    return Json(user);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {
        }
    }
}
