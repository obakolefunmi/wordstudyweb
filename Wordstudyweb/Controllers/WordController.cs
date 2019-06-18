using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Wordstudyweb.Models;



namespace Wordstudyweb.Controllers
{
    [RoutePrefix("api/Word")]
    public class WordController : ApiController
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();
        
        // GET: api/Word/Concepts
        [AllowAnonymous]
        [Route("Concepts")]
        public List<ConceptsData> GetConcepts()
        {
            List<Concept> concepts = db.Concepts.ToList();
            List<ConceptsData> data = new List<ConceptsData>();
            foreach(Concept c in concepts)
            {
                ConceptsData cd = new ConceptsData();

                cd.concept = c;
                cd.Topics = db.Topics.Where(j => j.ConceptId.Equals(c.Id)).ToList();
                if (!(cd.Topics.Count <= 0))
                {
                    data.Add(cd);
                }
           }
            return data;
        }

        // GET: api/Word/Concepts/5
        [Route("Concepts")]
        [ResponseType(typeof(Concept))]
        public IHttpActionResult GetConcept(Guid id)
        {
            Concept concept = db.Concepts.Find(id);
            if (concept == null)
            {
                return NotFound();
            }

            return Ok(concept);
        }

        // PUT: api/Word/Concepts/5
        [Authorize]
        [Route("Concepts")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConcept(Guid id, Concept concept)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != concept.Id)
            {
                return BadRequest();
            }

            db.Entry(concept).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptExists(id))
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

        // POST: api/Word/Concepts
        [AllowAnonymous]
        [Route("Concepts")]
        [ResponseType(typeof(Concept))]
        public IHttpActionResult PostConcept(Concept concept)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            concept.Id = Guid.NewGuid();
            db.Concepts.Add(concept);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ConceptExists(concept.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(concept);
        }

        // DELETE: api/Word/Concepts/5
        [Route("Concepts")]
        [ResponseType(typeof(Concept))]
        public IHttpActionResult DeleteConcept(Guid id)
        {
            Concept concept = db.Concepts.Find(id);
            if (concept == null)
            {
                return NotFound();
            }

            db.Concepts.Remove(concept);
            db.SaveChanges();

            return Ok(concept);
        }


        // GET: api/Word/Topics
        [Route("Topics")]
        public IQueryable<Topic> GetTopics()
        {
            return db.Topics;
        }

        // Get: api/Word/Topics/Sorted/5
        [Route("Topics/Sorted")]
        [ResponseType(typeof(Topic))]
        public IQueryable<Topic> GetTopicSorted(Guid id)
        {
            Concept concept = db.Concepts.Find(id);
           

            return db.Topics.Where(e => e.ConceptId.Equals(id));
        }

        // GET: api/Word/Topics/5
        [Route("Topics")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult GetTopic( Guid id)
        {
            Topic topic = db.Topics.Find(id);
          // Anchor anchor =  db.Anchors.Find(topic.AnchorId);
          //  TopicData td = new TopicData { Id = topic.Id, TimeStamp = topic.TimeStamp, Book = anchor.Book, Chapter = anchor.Chapter, Description = topic.Description, Message = topic.Message, Title = topic.Title, VerseFrom = anchor.VerseFrom, VersrTo = anchor.VersrTo, ConceptId = topic.ConceptId };
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        // PUT: api/Word/Topics/5
        [Authorize]

        [Route("Topics")]
        [ResponseType(typeof(void))]
        public  IHttpActionResult PutTopic(Guid id, TopicData topicdata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topicdata.Id)
            {
                return BadRequest(); 
            }
            Topic oldtopic = db.Topics.Find(id);
            Anchor oldanchor = db.Anchors.Find(oldtopic.AnchorId);
            
            oldtopic.ConceptId = topicdata.ConceptId;
            oldtopic.Description = topicdata.Description;
            oldtopic.Message = topicdata.Message;
            oldtopic.Title = topicdata.Title;
            oldanchor.Book = topicdata.Book;
            oldanchor.Chapter = topicdata.Chapter;
            oldanchor.VerseFrom = topicdata.VerseFrom;
            oldanchor.VersrTo = topicdata.VersrTo;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(id))
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
        [Authorize]
        [Route("Topics/Immage")]
        [HttpPost]
        public HttpResponseMessage PostImage()
        {

            HttpResponseMessage Uresult = null;

            string result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    string filePath;
                    var postedFile = httpRequest.Files[file];
                    switch (postedFile.ContentType.ToString())
                    {
                        case "image/png":
                            filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid() + ".png");
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                            break;
                        case "image/jpeg":
                            filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid() + ".jpeg");
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                            break;
                        case "image/jpg":
                            filePath = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid() + ".jpg");
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                            break;
                    }

                }
                result = docfiles.FirstOrDefault();

            }
            else
            {
                result = "C:\\Users\\Obakolefunnmi\\source\\repos\\wordstudyweb\\wordstusdyweb\\Images\\Topics\\default.jpg";
            }
            Uresult = Request.CreateResponse(HttpStatusCode.Created, result); ;
            return Uresult ;
        }

        // POST: api/Word/Topics

        [Route("Topics")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult PostTopic(TopicData topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            topic.Id = Guid.NewGuid();
            
            var newtopic = new Topic { Id = Guid.NewGuid(), AnchorId = Guid.NewGuid(), ConceptId = topic.ConceptId,TimeStamp = DateTime.UtcNow, Description = topic.Description, Title = topic.Title, Message = topic.Message,Url = topic.Url};
            var newanchor = new Anchor { Id = newtopic.AnchorId, TopicId = newtopic.Id, Book = topic.Book, Chapter = topic.Chapter, VerseFrom = topic.VerseFrom, VersrTo = topic.VersrTo };
            db.Topics.Add(newtopic);
            db.Anchors.Add(newanchor);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TopicExists(topic.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(newtopic);
        }

        // DELETE: api/Word/Topics
        [Authorize]
        [Route("Topics")]
        [ResponseType(typeof(Topic))]
        public IHttpActionResult DeleteTopic(Guid id)
        {
            Topic topic = db.Topics.Find(id);
            if (topic == null)
            {
                return NotFound();
            }

            db.Topics.Remove(topic);
            db.SaveChanges();

            return Ok(topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopicExists(Guid id)
        {
            return db.Topics.Count(e => e.Id == id) > 0;
        }  
               
        private bool ConceptExists(Guid id)
        {
            return db.Concepts.Count(e => e.Id == id) > 0;
        }
    }
}

