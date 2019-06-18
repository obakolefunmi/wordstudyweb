using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Wordstudyweb.Models;

namespace Wordstudyweb.Controllers
{
    public class CarouselsController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: api/Carousels
        public IQueryable<Carousel> GetCarousels()
        {
            return db.Carousels;
        }

        // GET: api/Carousels/5
        [ResponseType(typeof(Carousel))]
        public async Task<IHttpActionResult> GetCarousel(int id)
        {
            Carousel carousel = await db.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }

            return Ok(carousel);
        }

        // PUT: api/Carousels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCarousel(int id, Carousel carousel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carousel.Id)
            {
                return BadRequest();
            }

            db.Entry(carousel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarouselExists(id))
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

        // POST: api/Carousels
        [ResponseType(typeof(Carousel))]
        public async Task<IHttpActionResult> PostCarousel(Carousel carousel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Carousels.Add(carousel);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = carousel.Id }, carousel);
        }

        // DELETE: api/Carousels/5
        [ResponseType(typeof(Carousel))]
        public async Task<IHttpActionResult> DeleteCarousel(int id)
        {
            Carousel carousel = await db.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }

            db.Carousels.Remove(carousel);
            await db.SaveChangesAsync();

            return Ok(carousel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarouselExists(int id)
        {
            return db.Carousels.Count(e => e.Id == id) > 0;
        }
    }
}