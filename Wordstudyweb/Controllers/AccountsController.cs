using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Wordstudyweb.Models;
using Wordstudyweb.Providers;
using Wordstudyweb.Results;

namespace Wordstudyweb.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountsController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountsController()
        {
        }

        public AccountsController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        // GET api/Account/Profile
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Profile")]
        public Profile GetProfile()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            var usernanme = User.Identity.GetUserName();
            User user = db.Users.FirstOrDefault(e => e.MatriculationNumber == usernanme); //TO GET THE USER

            //var role = _userManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault()??"Member";// to get the users role


            return new Profile
            {
                Id = user.Id,
                FirstName = user.FirstName,

                LastName = user.LastName,

                MatriculationNumber = user.MatriculationNumber,

                Gender = user.Gender,

                Email = user.Email,

                PhoneNumber = user.PhoneNumber,

                DayOfBirth = user.DayOfBirth,

                Course = user.Course,

                Status = "Member"// role
            };
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------
        // GET api/Account/Profile?id=userid

        [Route("Profile")]
        public Profile GetProfile(Guid id)
        {
            User user = db.Users.FirstOrDefault(e => e.Id == id); //TO GET THE USER

            //var role = _userManager.GetRoles(user.Id.ToString()).FirstOrDefault();// to get the users role


            return new Profile
            {
                Id = user.Id,
                FirstName = user.FirstName,

                LastName = user.LastName,

                MatriculationNumber = user.MatriculationNumber,

                Gender = user.Gender,

                Email = user.Email,

                PhoneNumber = user.PhoneNumber,

                DayOfBirth = user.DayOfBirth,

                Course = user.Course,

                Status = "Member"//role
            };
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------
        // GET api/Account/Profile/Role
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Profile/Role")]
        public Roles GetProfileRole()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            User user = db.Users.FirstOrDefault(e => e.MatriculationNumber == User.Identity.GetUserName()); //TO GET THE USER

            string role = _userManager.GetRoles(User.Identity.GetUserId()).FirstOrDefault();// to get the users role
            switch (role)
            {
                case "Executive":
                    var exco = db.Excecutives.FirstOrDefault(e => e.UserId == user.Id);
                    return new Roles
                    {
                        Id = exco.Id,
                        UserId = exco.UserId,
                        RoomNumber = exco.RoomNumber,
                        Hall = exco.Hall,
                        Level = exco.Level,
                        Post = exco.Post,
                        Address = null,
                        PostHeld = null,
                        SchoolPeriod = null

                    };
                case "Member":
                    var member = db.Members.FirstOrDefault(e => e.UserId == user.Id);
                    return new Roles
                    {
                        Id = member.Id,
                        UserId = member.UserId,
                        RoomNumber = member.RoomNumber,
                        Hall = member.Hall,
                        Level = member.Level,
                        Post = null,
                        Address = null,
                        PostHeld = null,
                        SchoolPeriod = null

                    };
                case "Alumni":
                    var alumni = db.Alumni.FirstOrDefault(e => e.UserId == user.Id);
                    return new Roles
                    {
                        Id = alumni.Id,
                        UserId = alumni.UserId,
                        Address = alumni.Address,
                        PostHeld = alumni.PostHeld,
                        SchoolPeriod = alumni.SchoolPeriod,
                        Hall = null,
                        Level = null,
                        Post = null,
                        RoomNumber = null
                    };
                default:
                    return new Roles
                    {
                        Id = Guid.Empty,
                        UserId = user.Id,
                        Address = null,
                        PostHeld = null,
                        SchoolPeriod = null,
                        Hall = null,
                        Level = null,
                        Post = null,
                        RoomNumber = null
                    };
            }


        }

        //--------------------------------------------------------------------------------------------------------------------------
        // PUT api/Account/Profile?id=UserId
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Profile")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProfile(Guid id, EditProfile userprofile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userprofile.Id)
            {
                return BadRequest();
            }
            User olduser = db.Users.Find(id);

            olduser.Course = userprofile.Course;
            olduser.DayOfBirth = userprofile.DayOfBirth;
            olduser.FirstName = userprofile.FirstName;
            olduser.LastName = userprofile.LastName;
            olduser.PhoneNumber = userprofile.PhoneNumber;
            switch (userprofile.Status)
            {
                case "Executive":
                    var exco = db.Excecutives.FirstOrDefault(e => e.UserId == userprofile.Id);
                    exco.Hall = userprofile.Hall;
                    exco.Level = userprofile.Level;
                    exco.Post = userprofile.Post;
                    exco.RoomNumber = userprofile.RoomNumber;
                    break;

                case "Member":
                    var member = db.Members.FirstOrDefault(e => e.UserId == userprofile.Id);
                    member.Hall = userprofile.Hall;
                    member.Level = userprofile.Level;
                    member.RoomNumber = userprofile.RoomNumber;
                    break;
                case "Alumni":
                    var alumni = db.Alumni.FirstOrDefault(e => e.UserId == userprofile.Id);
                    alumni.Address = userprofile.Address;
                    alumni.PostHeld = userprofile.PostHeld;
                    alumni.SchoolPeriod = userprofile.SchoolPeriod;
                    break;

            }           // db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        //---------------------------------------------------------------------------------------------------------------------------------

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

    
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid id = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser() { Id = id.ToString(), UserName = model.MatricNo, Email = model.Email };
            User profile = new User() { Id = id, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, MatriculationNumber = model.MatricNo };


            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            db.Users.Add(profile);
            var settings = new Setting() { Id = Guid.NewGuid(), UserId = profile.Id, AutoDownloadOutlines = true, RemindMeetings = true };
            db.Settings.Add(settings);

            db.SaveChanges();
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        private bool UserExists(Guid id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
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