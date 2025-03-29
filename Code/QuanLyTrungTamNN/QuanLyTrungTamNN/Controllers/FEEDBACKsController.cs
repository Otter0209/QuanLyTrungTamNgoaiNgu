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
    public class FEEDBACKsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: FEEDBACKs
        public ActionResult Index()
        {
            var fEEDBACKs = db.FEEDBACKs.Include(f => f.ENROLLMENT).Include(f => f.TEACHER);
            return View(fEEDBACKs.ToList());
        }

        // GET: FEEDBACKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FEEDBACK fEEDBACK = db.FEEDBACKs.Find(id);
            if (fEEDBACK == null)
            {
                return HttpNotFound();
            }
            return View(fEEDBACK);
        }

        // GET: FEEDBACKs/Create
        public ActionResult Create()
        {
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID");
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName");
            return View();
        }

        // POST: FEEDBACKs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,EnrollmentID,TeacherID,Comments,Date")] FEEDBACK fEEDBACK)
        {
            if (ModelState.IsValid)
            {
                db.FEEDBACKs.Add(fEEDBACK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", fEEDBACK.EnrollmentID);
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", fEEDBACK.TeacherID);
            return View(fEEDBACK);
        }

        // GET: FEEDBACKs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FEEDBACK fEEDBACK = db.FEEDBACKs.Find(id);
            if (fEEDBACK == null)
            {
                return HttpNotFound();
            }
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", fEEDBACK.EnrollmentID);
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", fEEDBACK.TeacherID);
            return View(fEEDBACK);
        }

        // POST: FEEDBACKs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,EnrollmentID,TeacherID,Comments,Date")] FEEDBACK fEEDBACK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fEEDBACK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EnrollmentID = new SelectList(db.ENROLLMENTs, "EnrollmentID", "EnrollmentID", fEEDBACK.EnrollmentID);
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", fEEDBACK.TeacherID);
            return View(fEEDBACK);
        }

        // GET: FEEDBACKs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FEEDBACK fEEDBACK = db.FEEDBACKs.Find(id);
            if (fEEDBACK == null)
            {
                return HttpNotFound();
            }
            return View(fEEDBACK);
        }

        // POST: FEEDBACKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FEEDBACK fEEDBACK = db.FEEDBACKs.Find(id);
            db.FEEDBACKs.Remove(fEEDBACK);
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
