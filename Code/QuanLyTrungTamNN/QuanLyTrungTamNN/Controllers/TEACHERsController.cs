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
    public class TEACHERsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: TEACHERs
        public ActionResult Index()
        {
            return View(db.TEACHERs.ToList());
        }

        // GET: TEACHERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TEACHER tEACHER = db.TEACHERs.Find(id);
            if (tEACHER == null)
            {
                return HttpNotFound();
            }
            return View(tEACHER);
        }

        // GET: TEACHERs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TEACHERs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherID,FullName,PhoneNumber,Email,Expertise")] TEACHER tEACHER)
        {
            if (ModelState.IsValid)
            {
                db.TEACHERs.Add(tEACHER);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tEACHER);
        }

        // GET: TEACHERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TEACHER tEACHER = db.TEACHERs.Find(id);
            if (tEACHER == null)
            {
                return HttpNotFound();
            }
            return View(tEACHER);
        }

        // POST: TEACHERs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherID,FullName,PhoneNumber,Email,Expertise")] TEACHER tEACHER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tEACHER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tEACHER);
        }

        // GET: TEACHERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TEACHER tEACHER = db.TEACHERs.Find(id);
            if (tEACHER == null)
            {
                return HttpNotFound();
            }
            return View(tEACHER);
        }

        // POST: TEACHERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TEACHER tEACHER = db.TEACHERs.Find(id);
            db.TEACHERs.Remove(tEACHER);
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
