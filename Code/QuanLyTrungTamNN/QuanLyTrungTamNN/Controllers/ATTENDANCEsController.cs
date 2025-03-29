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
    public class ATTENDANCEsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: ATTENDANCEs
        public ActionResult Index()
        {
            var aTTENDANCEs = db.ATTENDANCEs.Include(a => a.ENROLLMENT);
            return View(aTTENDANCEs.ToList());
        }

        // GET: ATTENDANCEs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ATTENDANCE aTTENDANCE = db.ATTENDANCEs.Find(id);
            if (aTTENDANCE == null)
            {
                return HttpNotFound();
            }
            return View(aTTENDANCE);
        }

        // GET: ATTENDANCEs/Create
        public ActionResult Create()
        {
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID");
            return View();
        }

        // POST: ATTENDANCEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AttendanceID,EnrollmentID,Date,Status")] ATTENDANCE aTTENDANCE)
        {
            if (ModelState.IsValid)
            {
                db.ATTENDANCEs.Add(aTTENDANCE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", aTTENDANCE.EnrollmentID);
            return View(aTTENDANCE);
        }

        // GET: ATTENDANCEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ATTENDANCE aTTENDANCE = db.ATTENDANCEs.Find(id);
            if (aTTENDANCE == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", aTTENDANCE.EnrollmentID);
            return View(aTTENDANCE);
        }

        // POST: ATTENDANCEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttendanceID,EnrollmentID,Date,Status")] ATTENDANCE aTTENDANCE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aTTENDANCE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", aTTENDANCE.EnrollmentID);
            return View(aTTENDANCE);
        }

        // GET: ATTENDANCEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ATTENDANCE aTTENDANCE = db.ATTENDANCEs.Find(id);
            if (aTTENDANCE == null)
            {
                return HttpNotFound();
            }
            return View(aTTENDANCE);
        }

        // POST: ATTENDANCEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ATTENDANCE aTTENDANCE = db.ATTENDANCEs.Find(id);
            db.ATTENDANCEs.Remove(aTTENDANCE);
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
