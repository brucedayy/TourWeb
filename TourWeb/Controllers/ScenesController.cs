using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourWeb;

namespace TourWeb.Controllers
{
    public class ScenesController : Controller
    {
        private TourWebDBEntities db = new TourWebDBEntities();

        // GET: Scenes
        public async Task<ActionResult> Index()
        {
            return View(await db.Scene.ToListAsync());
        }

        // GET: Scenes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = await db.Scene.FindAsync(id);
            if (scene == null)
            {
                return HttpNotFound();
            }
            return View(scene);
        }

        // GET: Scenes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scenes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Sid,Img,Name,Position,Priority,Credit,Price,PubTime")] Scene scene)
        {
            if (ModelState.IsValid)
            {
                db.Scene.Add(scene);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(scene);
        }

        // GET: Scenes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = await db.Scene.FindAsync(id);
            if (scene == null)
            {
                return HttpNotFound();
            }
            return View(scene);
        }

        // POST: Scenes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Sid,Img,Name,Position,Priority,Credit,Price,PubTime")] Scene scene)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scene).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(scene);
        }

        // GET: Scenes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scene scene = await db.Scene.FindAsync(id);
            if (scene == null)
            {
                return HttpNotFound();
            }
            return View(scene);
        }

        // POST: Scenes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Scene scene = await db.Scene.FindAsync(id);
            db.Scene.Remove(scene);
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
