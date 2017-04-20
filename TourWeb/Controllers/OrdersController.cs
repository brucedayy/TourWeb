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
    public class OrdersController : Controller
    {
        private TourWebDBEntities db = new TourWebDBEntities();

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            var order = db.Order.Include(o => o.Scene).Include(o => o.User);
            return View(await order.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Order.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Sid = new SelectList(db.Scene, "Sid", "Img");
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Oid,Sid,UserName,Total,PubTime")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Order.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Sid = new SelectList(db.Scene, "Sid", "Img", order.Sid);
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", order.UserName);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Order.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Sid = new SelectList(db.Scene, "Sid", "Img", order.Sid);
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", order.UserName);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Oid,Sid,UserName,Total,PubTime")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Sid = new SelectList(db.Scene, "Sid", "Img", order.Sid);
            ViewBag.UserName = new SelectList(db.User, "UserName", "Password", order.UserName);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Order.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Order.FindAsync(id);
            db.Order.Remove(order);
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
