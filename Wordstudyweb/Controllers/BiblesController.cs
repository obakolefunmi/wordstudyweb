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
    public class BiblesController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: api/Bibles
        public IQueryable<Bible> GetBibles()
        {
            return db.Bibles;
        }

        // GET: api/Bibles/5
        [ResponseType(typeof(Bible))]
        public IHttpActionResult GetBible(Guid id)
        {
            Bible bible = db.Bibles.Find(id);
            if (bible == null)
            {
                return NotFound();
            }

            return Ok(bible);
        }

        // PUT: api/Bibles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBible(Guid id, Bible bible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bible.Id)
            {
                return BadRequest();
            }

            db.Entry(bible).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BibleExists(id))
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

        // POST: api/Bibles
        [ResponseType(typeof(Bible))]
        public IHttpActionResult PostBible(Bible bible)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bible.Id = Guid.NewGuid();

            db.Bibles.Add(bible);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BibleExists(bible.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(bible);
        }

        // DELETE: api/Bibles/5
        [ResponseType(typeof(Bible))]
        public IHttpActionResult DeleteBible(Guid id)
        {
            Bible bible = db.Bibles.Find(id);
            if (bible == null)
            {
                return NotFound();
            }

            db.Bibles.Remove(bible);
            db.SaveChanges();

            return Ok(bible);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BibleExists(Guid id)
        {
            return db.Bibles.Count(e => e.Id == id) > 0;
        }
    }
}