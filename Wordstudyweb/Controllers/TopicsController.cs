using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wordstudyweb.Models;

namespace Wordstudyweb.Controllers
{
    public class TopicsController : Controller
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: Topics
        public async Task<ActionResult> Index()
        {
            List<Concept> concepts = await db.Concepts.ToListAsync();
            List<ConceptData> cd = new List<ConceptData>();
            foreach(Concept i in concepts)
            {
                List<Topic> t = db.Topics.Where(o => o.ConceptId.Equals(i.Id)).ToList();
                Random random = new Random();
                ConceptData CC = new ConceptData();
                if (t.Count < 1)
                {
                    
                }
                else
                {
                    CC.concept = i;

                    CC.topic = t[random.Next(0, maxValue: t.Count - 1)];
                    cd.Add(CC);

                }
            }

            return View(cd);
        }

        // GET: Topics/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            BibleService bib = new BibleService();
            Common com = new Common();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            Anchor anchor = await db.Anchors.FindAsync(topic.AnchorId);
            ViewData["Text"] = await bib.GetScriptureKJV(anchor);
            ViewData["Date"] = com.Dateconverter(topic.TimeStamp.ToLocalTime());

            return View(topic);
        }


        public async Task<ActionResult> Conceptdetails(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concept concept = await db.Concepts.FindAsync(id);
            List<Topic> topics =  db.Topics.Where(t => t.ConceptId.Equals(id)).ToList();
            ViewData["Topics"] = topics;
            return View(concept);

        }

        // GET: Topics/Create
        [Authorize(Roles ="Executive")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ConceptId,AnchorId,Title,Message,Description,TimeStamp,Url,RowVersion")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                topic.Id = Guid.NewGuid();
                db.Topics.Add(topic);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(topic);
        }

        // GET: Topics/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ConceptId,AnchorId,Title,Message,Description,TimeStamp,Url,RowVersion")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topic).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = await db.Topics.FindAsync(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Topic topic = await db.Topics.FindAsync(id);
            db.Topics.Remove(topic);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
