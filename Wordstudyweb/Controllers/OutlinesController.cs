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
    public class OutlinesController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: api/Outlines
        public IQueryable<Outline> GetOutlines()
        {
            return db.Outlines;
        }

        // GET: api/Outlines/5
        [ResponseType(typeof(Outline))]
        public IHttpActionResult GetOutline(Guid id)
        {
            Outline outline = db.Outlines.Find(id);
            if (outline == null)
            {
                return NotFound();
            }

            return Ok(outline);
        }

        // PUT: api/Outlines/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOutline(Guid id, Outline outline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != outline.Id)
            {
                return BadRequest();
            }

            db.Entry(outline).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutlineExists(id))
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

        // POST: api/Outlines
        [ResponseType(typeof(Outline))]
        public IHttpActionResult PostOutline(Outline outline)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            outline.Id = Guid.NewGuid();
            outline.TimeStamp = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "W. Central Africa Standard Time");
            db.Outlines.Add(outline);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OutlineExists(outline.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(outline);
        }

        // DELETE: api/Outlines/5
        [ResponseType(typeof(Outline))]
        public IHttpActionResult DeleteOutline(Guid id)
        {
            Outline outline = db.Outlines.Find(id);
            if (outline == null)
            {
                return NotFound();
            }

            db.Outlines.Remove(outline);
            db.SaveChanges();

            return Ok(outline);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OutlineExists(Guid id)
        {
            return db.Outlines.Count(e => e.Id == id) > 0;
        }
    }
}