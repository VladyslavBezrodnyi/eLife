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
    public class ClinicAdminsController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/ClinicAdmins
        public IHttpActionResult GetClinicAdmins()
        {
            return Json(db.ClinicAdmins);
        }

        // GET: api/ClinicAdmins/5
        [ResponseType(typeof(ClinicAdmin))]
        public IHttpActionResult GetClinicAdmin(int id)
        {
            ClinicAdmin clinicAdmin = db.ClinicAdmins.Find(id);
            if (clinicAdmin == null)
            {
                return NotFound();
            }

            return Json(clinicAdmin);
        }

        // PUT: api/ClinicAdmins/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClinicAdmin(int id, ClinicAdmin clinicAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != clinicAdmin.Id_clinic_admin)
            {
                return BadRequest();
            }

            db.Entry(clinicAdmin).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClinicAdminExists(id))
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

        // POST: api/ClinicAdmins
        [ResponseType(typeof(ClinicAdmin))]
        public IHttpActionResult PostClinicAdmin(ClinicAdmin clinicAdmin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ClinicAdmins.Add(clinicAdmin);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = clinicAdmin.Id_clinic_admin }, clinicAdmin);
        }

        // DELETE: api/ClinicAdmins/5
        [ResponseType(typeof(ClinicAdmin))]
        public IHttpActionResult DeleteClinicAdmin(int id)
        {
            ClinicAdmin clinicAdmin = db.ClinicAdmins.Find(id);
            if (clinicAdmin == null)
            {
                return NotFound();
            }

            db.ClinicAdmins.Remove(clinicAdmin);
            db.SaveChanges();

            return Ok(clinicAdmin);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClinicAdminExists(int id)
        {
            return db.ClinicAdmins.Count(e => e.Id_clinic_admin == id) > 0;
        }
    }
}