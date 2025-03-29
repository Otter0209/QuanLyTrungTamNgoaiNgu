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
    public class CLASSesController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: CLASSes
        public ActionResult Index()
        {
            var cLASSes = db.CLASSes.Include(c => c.TEACHER);
            return View(cLASSes.ToList());
        }

        // GET: CLASSes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS cLASS = db.CLASSes.Find(id);
            if (cLASS == null)
            {
                return HttpNotFound();
            }
            return View(cLASS);
        }

        // GET: CLASSes/Create
        public ActionResult Create()
        {
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName");
            return View();
        }

        // POST: CLASSes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassID,ClassName,Schedule,Location,TeacherID")] CLASS cLASS)
        {
            if (ModelState.IsValid)
            {
                db.CLASSes.Add(cLASS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cLASS.TeacherID);
            return View(cLASS);
        }

        // GET: CLASSes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS cLASS = db.CLASSes.Find(id);
            if (cLASS == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cLASS.TeacherID);
            return View(cLASS);
        }

        // POST: CLASSes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassID,ClassName,Schedule,Location,TeacherID")] CLASS cLASS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLASS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cLASS.TeacherID);
            return View(cLASS);
        }

        // GET: CLASSes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLASS cLASS = db.CLASSes.Find(id);
            if (cLASS == null)
            {
                return HttpNotFound();
            }
            return View(cLASS);
        }

        // POST: CLASSes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CLASS cLASS = db.CLASSes.Find(id);
            db.CLASSes.Remove(cLASS);
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
