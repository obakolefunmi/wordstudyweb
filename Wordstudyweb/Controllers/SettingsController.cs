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
    public class SettingsController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: api/Settings
        public IQueryable<Setting> GetSettings()
        {
            return db.Settings;
        }

        // GET: api/Settings/5
        [ResponseType(typeof(Setting))]
        public IHttpActionResult GetSetting(Guid id)
        {
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return NotFound();
            }

            return Ok(setting);
        }

        // PUT: api/Settings/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSetting(Guid id, Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != setting.Id)
            {
                return BadRequest();
            }

            db.Entry(setting).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
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

        // POST: api/Settings
        [ResponseType(typeof(Setting))]
        public IHttpActionResult PostSetting(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            setting.Id = Guid.NewGuid();
            db.Settings.Add(setting);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SettingExists(setting.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = setting.Id }, setting);
        }

        // DELETE: api/Settings/5
        [ResponseType(typeof(Setting))]
        public IHttpActionResult DeleteSetting(Guid id)
        {
            Setting setting = db.Settings.Find(id);
            if (setting == null)
            {
                return NotFound();
            }

            db.Settings.Remove(setting);
            db.SaveChanges();

            return Ok(setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SettingExists(Guid id)
        {
            return db.Settings.Count(e => e.Id == id) > 0;
        }
    }
}