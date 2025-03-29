using System.Net;
using System.Linq;
using System.Web.Mvc;
using QuanLyTrungTamNN.Models;

namespace QuanLyTrungTamNN.Controllers
{
    public class StudentsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.STUDENTs.ToList();
            return View(students);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(STUDENT student)
        {
            if (ModelState.IsValid)
            {
                db.STUDENTs.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            STUDENT student = db.STUDENTs.Find(id);
            if (student == null)
                return HttpNotFound();
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(STUDENT student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            STUDENT student = db.STUDENTs.Find(id);
            if (student == null)
                return HttpNotFound();
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            STUDENT student = db.STUDENTs.Find(id);
            db.STUDENTs.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
