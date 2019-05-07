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
    public class DBController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/DB
        public IQueryable<Clinic> GetClinics()
        {
            return db.Clinics;
        }

        // GET: api/DB/5
        [ResponseType(typeof(Clinic))]
        public IHttpActionResult GetClinic(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return NotFound();
            }

            return Ok(clinic);
        }

        // PUT: api/DB/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClinic(int id, Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clinic.Id_clinic)
            {
                return BadRequest();
            }

            db.Entry(clinic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicExists(id))
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

        // POST: api/DB
        [ResponseType(typeof(Clinic))]
        public IHttpActionResult PostClinic(Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clinics.Add(clinic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = clinic.Id_clinic }, clinic);
        }

        // DELETE: api/DB/5
        [ResponseType(typeof(Clinic))]
        public IHttpActionResult DeleteClinic(int id)
        {
            Clinic clinic = db.Clinics.Find(id);
            if (clinic == null)
            {
                return NotFound();
            }

            db.Clinics.Remove(clinic);
            db.SaveChanges();

            return Ok(clinic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClinicExists(int id)
        {
            return db.Clinics.Count(e => e.Id_clinic == id) > 0;
        }
    }
}