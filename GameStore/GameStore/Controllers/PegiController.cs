using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class PegiController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pegi
        public ActionResult Index()
        {
            return View(db.Pegi.ToList());
        }

        // GET: Pegi/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pegi pegi = db.Pegi.Find(id);
            if (pegi == null)
            {
                return HttpNotFound();
            }
            return View(pegi);
        }

        // GET: Pegi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pegi/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ShortName,Description,IconPath")] Pegi pegi)
        {
            if (ModelState.IsValid)
            {
                db.Pegi.Add(pegi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pegi);
        }

        // GET: Pegi/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pegi pegi = db.Pegi.Find(id);
            if (pegi == null)
            {
                return HttpNotFound();
            }
            return View(pegi);
        }

        // POST: Pegi/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ShortName,Description,IconPath")] Pegi pegi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pegi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pegi);
        }

        // GET: Pegi/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pegi pegi = db.Pegi.Find(id);
            if (pegi == null)
            {
                return HttpNotFound();
            }
            return View(pegi);
        }

        // POST: Pegi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pegi pegi = db.Pegi.Find(id);
            db.Pegi.Remove(pegi);
            db.SaveChanges();
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
