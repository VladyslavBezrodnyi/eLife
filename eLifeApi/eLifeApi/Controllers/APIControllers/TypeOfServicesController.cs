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

namespace eLifeApi.Controllers
{
    public class TypeOfServicesController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/TypeOfServices
        public IHttpActionResult GetTypeOfServices()
        {
            return Json(db.TypeOfServices);
        }

        // GET: api/TypeOfServices/5
        [ResponseType(typeof(TypeOfService))]
        public IHttpActionResult GetTypeOfService(int id)
        {
            TypeOfService typeOfService = db.TypeOfServices.Find(id);
            if (typeOfService == null)
            {
                return NotFound();
            }

            return Json(typeOfService);
        }

        // PUT: api/TypeOfServices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTypeOfService(int id, TypeOfService typeOfService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeOfService.Id_type)
            {
                return BadRequest();
            }

            db.Entry(typeOfService).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeOfServiceExists(id))
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

        // POST: api/TypeOfServices
        [ResponseType(typeof(TypeOfService))]
        public IHttpActionResult PostTypeOfService(TypeOfService typeOfService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TypeOfServices.Add(typeOfService);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TypeOfServiceExists(typeOfService.Id_type))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = typeOfService.Id_type }, typeOfService);
        }

        // DELETE: api/TypeOfServices/5
        [ResponseType(typeof(TypeOfService))]
        public IHttpActionResult DeleteTypeOfService(int id)
        {
            TypeOfService typeOfService = db.TypeOfServices.Find(id);
            if (typeOfService == null)
            {
                return NotFound();
            }

            db.TypeOfServices.Remove(typeOfService);
            db.SaveChanges();

            return Ok(typeOfService);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TypeOfServiceExists(int id)
        {
            return db.TypeOfServices.Count(e => e.Id_type == id) > 0;
        }
    }
}