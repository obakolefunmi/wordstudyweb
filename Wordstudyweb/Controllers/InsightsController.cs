using Microsoft.AspNet.Identity;
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
    [RoutePrefix("api/Insights")]
    public class InsightsController : ApiController
    {

        private Wordstudywebmodels db = new Wordstudywebmodels();
        private ApplicationUserManager _userManager;

        // GET: api/Insights/Insight
        [Route("Insight")]
        public List<InsightData> GetInsights()
        {
            List<Insight> insights = db.Insights.ToList();
            List <InsightData> insightDatas = new List<InsightData>();
            foreach(Insight i in insights)
            {
                InsightData d = new InsightData();
               // var role = _userManager.GetRoles(i.UserId.ToString()).FirstOrDefault();// to get the users role

                d.Id = i.Id;
                d.insight = i.insight;
                d.RowVersion = i.RowVersion;
                d.TimeStamp = i.TimeStamp;
                d.Url = i.Url;
                d.User = db.Users.Find(i.UserId);
                d.Status = "Member";// role;
                d.Reactions = db.Reactions.Where(m => m.UserId.Equals(i.UserId)).Count();
                insightDatas.Add(d);
            }
            return  insightDatas;
        }

        // GET: api/Insights/Insight?id=InsightId
        [Route("Insight")]
        [ResponseType(typeof(Insight))]
        public IHttpActionResult GetInsight(Guid id)
        {
            Insight insight = db.Insights.Find(id);
            InsightData insightdata = new InsightData();

            if (insight == null)
            {
                return NotFound();
            }
            User user = db.Users.Find(insight.UserId);
            //var role = _userManager.GetRoles(user.Id.ToString()).FirstOrDefault();// to get the users role

            insightdata.Id = insight.Id;
            insightdata.insight = insight.insight;
            insightdata.RowVersion = insight.RowVersion;
            insightdata.TimeStamp = insight.TimeStamp;
            insightdata.Url = insight.Url;
            insightdata.User = user;
            insightdata.Reactions = db.Reactions.Where(m => m.UserId.Equals(insight.UserId)).Count();

            insightdata.Status = "Member";// role;


           

            return Ok(insightdata);
        }
        // GET: api/Insights/Insight/User?id=UserId
        [Route("Insight/User")]
        [ResponseType(typeof(List<Insight>))]
        public IQueryable<Insight> GetUserInsights(Guid id)
        {
            return db.Insights.Where(e => e.UserId == id);
        }

        // PUT: api/Insights/Insight?id=InsightId
        [Route("Insight")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutInsight(Guid id, Insight insight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != insight.Id)
            {
                return BadRequest();
            }

            db.Entry(insight).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsightExists(id))
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

        // POST: api/Insights/Insight
        [Route("Insight")]
        [ResponseType(typeof(Insight))]
        public IHttpActionResult PostInsight(Insight insight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid theId = Guid.NewGuid();
            string black = insight.insight;
            insight.Id = theId;
            insight.TimeStamp =   //TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, "W. Central Africa Standard Time");
                DateTime.UtcNow;
            db.Insights.Add(insight);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (InsightExists(insight.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(insight);
        }

        // DELETE: api/Insights/Insight
        [Route("Insight")]
        [ResponseType(typeof(Insight))]
        public IHttpActionResult DeleteInsight(Guid id)
        {
            Insight insight = db.Insights.Find(id);
            if (insight == null)
            {
                return NotFound();
            }

            db.Insights.Remove(insight);
            db.SaveChanges();

            return Ok(insight);
        }



        

        // GET: api/Insights/Reactions
        [Route("Reactions")]
        public IQueryable<Reaction> GetReactions()
        {
            return db.Reactions;
        }

        // GET: api/Insights/Reactions?id=ReactionId
        [Route("Reactions")]
        [ResponseType(typeof(Reaction))]
        public IHttpActionResult GetReaction(Guid id)
        {
            Reaction reaction = db.Reactions.Find(id);
            if (reaction == null)
            {
                return NotFound();
            }

            return Ok(reaction);
        }

        // GET: api/Insights/Reactions?id=InsightId
        [Route("Reactions/Insight")]
        public IQueryable<Reaction> GetInsightReactions(Guid id)
        {
            return db.Reactions.Where(e => e.InsightId == id);
        }
        // GET: api/Insights/User?id=UserId
        [Route("Reactions/User")]
        public IQueryable<Reaction> GetUserReactions(Guid id)
        {
            return db.Reactions.Where(e => e.UserId == id);
        }
        // PUT: api/Insights/Reactions?id=ReactionId
        [Route("Reactions")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReaction(Guid id, Reaction reaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reaction.Id)
            {
                return BadRequest();
            }

            db.Entry(reaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReactionExists(id))
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

        // POST: api/Insights/Reactions
        [Route("Reactions")]
        [ResponseType(typeof(Reaction))]
        public IHttpActionResult PostReaction(Reaction reaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            reaction.Id = Guid.NewGuid();

            db.Reactions.Add(reaction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ReactionExists(reaction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(reaction);
        }

        // DELETE: api/Insights/Reactions?id=ReactionId
        [Route("Reactions")]
        [ResponseType(typeof(Reaction))]
        public IHttpActionResult DeleteReaction(Guid id)
        {
            Reaction reaction = db.Reactions.Find(id);
            if (reaction == null)
            {
                return NotFound();
            }

            db.Reactions.Remove(reaction);
            db.SaveChanges();

            return Ok(reaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReactionExists(Guid id)
        {
            return db.Reactions.Count(e => e.Id == id) > 0;
        }
    
        private bool InsightExists(Guid id)
        {
            return db.Insights.Count(e => e.Id == id) > 0;
        }
    }
}
