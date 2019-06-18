using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Wordstudyweb.Models;

namespace Wordstudyweb.Controllers
{
    
    [Authorize]
    public class AnnouncementsController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();
        // GET: api/Announcements?username=shalom
        [AllowAnonymous]
        public HttpResponseMessage Get( string username)
      {

           // ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
          //  User user = db.Users.FirstOrDefault(e => e.MatriculationNumber == User.Identity.GetUserName()); //TO GET THE USER
            //AnnouncementsWebSocketHandler socketHandler = new AnnouncementsWebSocketHandler(username);
            //HttpContext.Current.AcceptWebSocketRequest(new AnnouncementsWebSocketHandler(username));
            //socketHandler.getallannouncements(username);
        return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        public IQueryable<Announcement> GetAnnouncements()
        {
            return db.Announcements;
        }
        // GET: api/Announcements?id=announcementid
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult GetAnnouncement(Guid id)
        {
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return Ok(announcement);
        }

        // PUT: api/Announcements?id=announcementid
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnnouncement(Guid id, Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != announcement.Id)
            {
                return BadRequest();
            }

            db.Entry(announcement).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
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

        // POST: api/Announcements
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult PostAnnouncement(Announcement announcement)

        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            User user = db.Users.FirstOrDefault(e => e.MatriculationNumber == User.Identity.GetUserName()); //TO GET THE USER

            //AnnouncementsWebSocketHandler socketHandler = new AnnouncementsWebSocketHandler("");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            announcement.Id = Guid.NewGuid();
            db.Announcements.Add(announcement);

            try
            {
                db.SaveChanges();
              //  socketHandler.Announcementshare(announcement);

            }
            catch (DbUpdateException)
            {
                if (AnnouncementExists(announcement.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(announcement);
        }

        // DELETE: api/Announcements?id=announcementid
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult DeleteAnnouncement(Guid id)
        {
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return NotFound();
            }

            db.Announcements.Remove(announcement);
            db.SaveChanges();

            return Ok(announcement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnnouncementExists(Guid id)
        {
            return db.Announcements.Count(e => e.Id == id) > 0;
        }
        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }

}