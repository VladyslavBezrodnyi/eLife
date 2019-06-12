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
    public class DoctorInformsController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/DoctorInforms
        public IHttpActionResult GetDoctorInforms()
        {
            return Json(db.DoctorInforms);
        }

        // GET: api/DoctorInforms/5
        [ResponseType(typeof(DoctorInform))]
        public IHttpActionResult GetDoctorInform(int id)
        {
            DoctorInform doctorInform = db.DoctorInforms.Find(id);
            if (doctorInform == null)
            {
                return NotFound();
            }

            return Json(doctorInform);
        }

        // PUT: api/DoctorInforms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctorInform(int id, DoctorInform doctorInform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorInform.Id)
            {
                return BadRequest();
            }

            db.Entry(doctorInform).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorInformExists(id))
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

        // POST: api/DoctorInforms
        [ResponseType(typeof(DoctorInform))]
        public IHttpActionResult PostDoctorInform(DoctorInform doctorInform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DoctorInforms.Add(doctorInform);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctorInform.Id }, doctorInform);
        }

        // DELETE: api/DoctorInforms/5
        [ResponseType(typeof(DoctorInform))]
        public IHttpActionResult DeleteDoctorInform(int id)
        {
            DoctorInform doctorInform = db.DoctorInforms.Find(id);
            if (doctorInform == null)
            {
                return NotFound();
            }

            db.DoctorInforms.Remove(doctorInform);
            db.SaveChanges();

            return Ok(doctorInform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorInformExists(int id)
        {
            return db.DoctorInforms.Count(e => e.Id == id) > 0;
        }
    }
}