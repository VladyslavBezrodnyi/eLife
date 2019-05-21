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
    public class ConversationRepliesController : ApiController
    {
        private eLifeDB db = new eLifeDB();

        // GET: api/ConversationReplies
        public IHttpActionResult GetConversationReplies()
        {
            return Json(db.ConversationReplies);
        }

        // GET: api/ConversationReplies/5
        [ResponseType(typeof(ConversationReply))]
        public IHttpActionResult GetConversationReply(int id)
        {
            ConversationReply conversationReply = db.ConversationReplies.Find(id);
            if (conversationReply == null)
            {
                return NotFound();
            }

            return Json(conversationReply);
        }

        // PUT: api/ConversationReplies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConversationReply(int id, ConversationReply conversationReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != conversationReply.Id_reply)
            {
                return BadRequest();
            }

            db.Entry(conversationReply).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationReplyExists(id))
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

        // POST: api/ConversationReplies
        [ResponseType(typeof(ConversationReply))]
        public IHttpActionResult PostConversationReply(ConversationReply conversationReply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ConversationReplies.Add(conversationReply);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = conversationReply.Id_reply }, conversationReply);
        }

        // DELETE: api/ConversationReplies/5
        [ResponseType(typeof(ConversationReply))]
        public IHttpActionResult DeleteConversationReply(int id)
        {
            ConversationReply conversationReply = db.ConversationReplies.Find(id);
            if (conversationReply == null)
            {
                return NotFound();
            }

            db.ConversationReplies.Remove(conversationReply);
            db.SaveChanges();

            return Ok(conversationReply);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationReplyExists(int id)
        {
            return db.ConversationReplies.Count(e => e.Id_reply == id) > 0;
        }
    }
}