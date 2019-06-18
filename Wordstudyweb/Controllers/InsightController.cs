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
{[Authorize]
    public class InsightController : Controller
    {
        private Wordstudywebmodels db = new Wordstudywebmodels();

        // GET: Insight
        public async Task<ActionResult> Index()
        {
            return View(await db.Insights.ToListAsync());
        }

        // GET: Insight/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = await db.Insights.FindAsync(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // GET: Insight/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insight/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,insight,Url,TimeStamp,RowVersion")] Insight insight)
        {
            if (ModelState.IsValid)
            {
                insight.Id = Guid.NewGuid();
                db.Insights.Add(insight);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(insight);
        }

        // GET: Insight/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = await db.Insights.FindAsync(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // POST: Insight/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,insight,Url,TimeStamp,RowVersion")] Insight insight)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insight).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(insight);
        }

        // GET: Insight/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = await db.Insights.FindAsync(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // POST: Insight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Insight insight = await db.Insights.FindAsync(id);
            db.Insights.Remove(insight);
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
