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
    public class ENROLLMENTsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: ENROLLMENTs
        public ActionResult Index()
        {
            var eNROLLMENTs = db.ENROLLMENTs.Include(e => e.CLASS).Include(e => e.STUDENT);
            return View(eNROLLMENTs.ToList());
        }

        // GET: ENROLLMENTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENROLLMENT eNROLLMENT = db.ENROLLMENTs.Find(id);
            if (eNROLLMENT == null)
            {
                return HttpNotFound();
            }
            return View(eNROLLMENT);
        }

        // GET: ENROLLMENTs/Create
        public ActionResult Create()
        {
            ViewBag.ClassID = new SelectList(db.CLASSes, "ClassID", "ClassName");
            ViewBag.StudentID = new SelectList(db.STUDENTs, "StudentID", "FullName");
            return View();
        }

        // POST: ENROLLMENTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,StudentID,ClassID,RegistrationDate")] ENROLLMENT eNROLLMENT)
        {
            if (ModelState.IsValid)
            {
                db.ENROLLMENTs.Add(eNROLLMENT);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassID = new SelectList(db.CLASSes, "ClassID", "ClassName", eNROLLMENT.ClassID);
            ViewBag.StudentID = new SelectList(db.STUDENTs, "StudentID", "FullName", eNROLLMENT.StudentID);
            return View(eNROLLMENT);
        }

        // GET: ENROLLMENTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENROLLMENT eNROLLMENT = db.ENROLLMENTs.Find(id);
            if (eNROLLMENT == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassID = new SelectList(db.CLASSes, "ClassID", "ClassName", eNROLLMENT.ClassID);
            ViewBag.StudentID = new SelectList(db.STUDENTs, "StudentID", "FullName", eNROLLMENT.StudentID);
            return View(eNROLLMENT);
        }

        // POST: ENROLLMENTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,StudentID,ClassID,RegistrationDate")] ENROLLMENT eNROLLMENT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eNROLLMENT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassID = new SelectList(db.CLASSes, "ClassID", "ClassName", eNROLLMENT.ClassID);
            ViewBag.StudentID = new SelectList(db.STUDENTs, "StudentID", "FullName", eNROLLMENT.StudentID);
            return View(eNROLLMENT);
        }

        // GET: ENROLLMENTs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENROLLMENT eNROLLMENT = db.ENROLLMENTs.Find(id);
            if (eNROLLMENT == null)
            {
                return HttpNotFound();
            }
            return View(eNROLLMENT);
        }

        // POST: ENROLLMENTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ENROLLMENT eNROLLMENT = db.ENROLLMENTs.Find(id);
            db.ENROLLMENTs.Remove(eNROLLMENT);
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
