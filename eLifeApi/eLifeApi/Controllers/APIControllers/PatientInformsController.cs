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
    public class PatientInformsController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/PatientInforms
        public IHttpActionResult GetPatientInforms()
        {
            return Json(db.PatientInforms);
        }

        // GET: api/PatientInforms/5
        [ResponseType(typeof(PatientInform))]
        public IHttpActionResult GetPatientInform(int id)
        {
            PatientInform patientInform = db.PatientInforms.Find(id);
            if (patientInform == null)
            {
                return NotFound();
            }

            return Json(patientInform);
        }

        // PUT: api/PatientInforms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatientInform(int id, PatientInform patientInform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patientInform.PatientInfoId)
            {
                return BadRequest();
            }

            db.Entry(patientInform).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientInformExists(id))
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

        // POST: api/PatientInforms
        [ResponseType(typeof(PatientInform))]
        public IHttpActionResult PostPatientInform(PatientInform patientInform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PatientInforms.Add(patientInform);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patientInform.PatientInfoId }, patientInform);
        }

        // DELETE: api/PatientInforms/5
        [ResponseType(typeof(PatientInform))]
        public IHttpActionResult DeletePatientInform(int id)
        {
            PatientInform patientInform = db.PatientInforms.Find(id);
            if (patientInform == null)
            {
                return NotFound();
            }

            db.PatientInforms.Remove(patientInform);
            db.SaveChanges();

            return Ok(patientInform);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientInformExists(int id)
        {
            return db.PatientInforms.Count(e => e.PatientInfoId == id) > 0;
        }
    }
}