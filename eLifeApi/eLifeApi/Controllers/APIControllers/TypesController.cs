using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using eLifeApi.Models;
using Type = eLifeApi.Models.Type;

namespace eLifeApi.Controllers
{
    public class TypesController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/Types
        public IHttpActionResult GetTypes()
        {
            return Json(db.Types);
        }

        // GET: api/Types/5
        [ResponseType(typeof(Type))]
        public IHttpActionResult GetType(int id)
        {
            Type type = db.Types.Find(id);
            if (type == null)
            {
                return NotFound();
            }

            return Json(type);
        }

        // PUT: api/Types/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutType(int id, Type type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != type.Id_type)
            {
                return BadRequest();
            }

            db.Entry(type).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Types
        [ResponseType(typeof(Type))]
        public IHttpActionResult PostType(Type type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Types.Add(type);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = type.Id_type }, type);
        }

        // DELETE: api/Types/5
        [ResponseType(typeof(Type))]
        public IHttpActionResult DeleteType(int id)
        {
            Type type = db.Types.Find(id);
            if (type == null)
            {
                return NotFound();
            }

            db.Types.Remove(type);
            db.SaveChanges();

            return Ok(type);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeExists(int id)
        {
            return db.Types.Count(e => e.Id_type == id) > 0;
        }
    }
}