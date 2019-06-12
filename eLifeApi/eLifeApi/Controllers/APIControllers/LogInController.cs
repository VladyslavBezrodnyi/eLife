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
    public class LogInController : ApiController
    {

        eLifeDB db = new eLifeDB();

        // GET: api/LogIn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LogIn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LogIn
        [HttpPost]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            
                User user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Email == model.Email);
                if(user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest();
                }
            
        }

        // PUT: api/LogIn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LogIn/5
        public void Delete(int id)
        {
        }
    }
}
