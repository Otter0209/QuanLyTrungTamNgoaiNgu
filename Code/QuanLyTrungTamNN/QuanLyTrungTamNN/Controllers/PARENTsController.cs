using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTrungTamNN.Models;

namespace QuanLyTrungTamNN.Controllers
{
    public class PARENTsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: PARENTs
        public ActionResult Index()
        {
            return View(db.PARENTs.ToList());
        }

        // GET: PARENTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARENT pARENT = db.PARENTs.Find(id);
            if (pARENT == null)
            {
                return HttpNotFound();
            }
            return View(pARENT);
        }

        // GET: PARENTs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PARENTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParentID,FullName,PhoneNumber,Email")] PARENT pARENT)
        {
            if (ModelState.IsValid)
            {
                db.PARENTs.Add(pARENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pARENT);
        }

        // GET: PARENTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARENT pARENT = db.PARENTs.Find(id);
            if (pARENT == null)
            {
                return HttpNotFound();
            }
            return View(pARENT);
        }

        // POST: PARENTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParentID,FullName,PhoneNumber,Email")] PARENT pARENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pARENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pARENT);
        }

        // GET: PARENTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PARENT pARENT = db.PARENTs.Find(id);
            if (pARENT == null)
            {
                return HttpNotFound();
            }
            return View(pARENT);
        }

        // POST: PARENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PARENT pARENT = db.PARENTs.Find(id);
            db.PARENTs.Remove(pARENT);
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
