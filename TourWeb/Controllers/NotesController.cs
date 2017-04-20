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
    public class NotesController : Controller
    {
        private TourWebDBEntities db = new TourWebDBEntities();

        // GET: Notes
        public async Task<ActionResult> Index()
        {
            var note = db.Note.Include(n => n.User);
            return View(await note.ToListAsync());
        }

        // GET: Notes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await db.Note.FindAsync(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password");
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Nid,UserName,Details,PubTime")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Note.Add(note);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", note.UserName);
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await db.Note.FindAsync(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", note.UserName);
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Nid,UserName,Details,PubTime")] Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", note.UserName);
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = await db.Note.FindAsync(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Note note = await db.Note.FindAsync(id);
            db.Note.Remove(note);
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
