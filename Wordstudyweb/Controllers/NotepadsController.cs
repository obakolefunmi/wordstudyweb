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
using Wordstudyweb.Models;

namespace Wordstudyweb.Controllers
{
    public class NotepadsController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: api/Notepads
        public IQueryable<Notepad> GetNotepads()
        {
            return db.Notepads;
        }

        // GET: api/Notepads/5
        [ResponseType(typeof(Notepad))]
        public IHttpActionResult GetNotepad(Guid id)
        {
            Notepad notepad = db.Notepads.Find(id);
            if (notepad == null)
            {
                return NotFound();
            }

            return Ok(notepad);
        }

        // PUT: api/Notepads/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNotepad(Guid id, Notepad notepad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notepad.Id)
            {
                return BadRequest();
            }

            db.Entry(notepad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotepadExists(id))
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

        // POST: api/Notepads
        [ResponseType(typeof(Notepad))]
        public IHttpActionResult PostNotepad(Notepad notepad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notepads.Add(notepad);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (NotepadExists(notepad.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(notepad);
        }

        // DELETE: api/Notepads/5
        [ResponseType(typeof(Notepad))]
        public IHttpActionResult DeleteNotepad(Guid id)
        {
            Notepad notepad = db.Notepads.Find(id);
            if (notepad == null)
            {
                return NotFound();
            }

            db.Notepads.Remove(notepad);
            db.SaveChanges();

            return Ok(notepad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotepadExists(Guid id)
        {
            return db.Notepads.Count(e => e.Id == id) > 0;
        }
    }
}