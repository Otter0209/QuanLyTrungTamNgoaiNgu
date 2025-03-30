using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyTrungTamNN.Models;
using QuanLyTrungTamNN.ViewModels;

namespace QuanLyTrungTamNN.Controllers
{
    public class STUDENTsController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: STUDENTs
        public ActionResult Index()
        {
            // Nếu cần lấy thông tin PARENT, sử dụng Include
            var students = db.STUDENTs.Include(s => s.PARENT).ToList();
            return View(students);
        }

        // GET: STUDENTs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            STUDENT student = db.STUDENTs.Find(id);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // GET: STUDENTs/Create
        public ActionResult Create()
        {
            var model = new StudentParentViewModel();
            return View(model);
        }


        // POST: STUDENTs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra ngày sinh có hợp lệ không
                if (model.DateOfBirth == null)
                {
                    ModelState.AddModelError("DateOfBirth", "Ngày sinh không được để trống.");
                    return View(model);
                }

                // Tính tuổi của học viên
                int age = DateTime.Now.Year - model.DateOfBirth.Value.Year;
                if (DateTime.Now < model.DateOfBirth.Value.AddYears(age))
                {
                    age--; // Điều chỉnh nếu chưa đến sinh nhật
                }

                // Kiểm tra nếu tuổi < 6 thì báo lỗi
                if (age < 6)
                {
                    ModelState.AddModelError("DateOfBirth", "Học viên phải từ 6 tuổi trở lên.");
                    return View(model);
                }

                // 1) Tạo đối tượng PARENT mới
                var parent = new PARENT
                {
                    FullName = model.ParentFullName,
                    PhoneNumber = model.ParentPhone,
                    Email = model.ParentEmail
                };
                db.PARENTs.Add(parent);
                db.SaveChanges();

                // 2) Tạo STUDENT mới, gán ParentID = parent vừa tạo
                var student = new STUDENT
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    ParentID = parent.ParentID
                };
                db.STUDENTs.Add(student);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // Nếu ModelState không hợp lệ, trả lại view để sửa
            return View(model);
        }


        // GET: STUDENTs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            STUDENT student = db.STUDENTs.Find(id);
            if (student == null)
                return HttpNotFound();

            ViewBag.ParentID = new SelectList(db.PARENTs, "ParentID", "FullName", student.ParentID);
            return View(student);
        }

        // POST: STUDENTs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,FullName,PhoneNumber,Email,DateOfBirth,Address,ParentID")] STUDENT student)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra ngày sinh có hợp lệ không
                if (student.DateOfBirth == null)
                {
                    ModelState.AddModelError("DateOfBirth", "Ngày sinh không được để trống.");
                }
                else
                {
                    int age = DateTime.Now.Year - student.DateOfBirth.Value.Year;
                    if (DateTime.Now < student.DateOfBirth.Value.AddYears(age))
                    {
                        age--; // Điều chỉnh nếu chưa đến sinh nhật
                    }

                    if (age < 6)
                    {
                        ModelState.AddModelError("DateOfBirth", "Học viên phải từ 6 tuổi trở lên.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.ParentID = new SelectList(db.PARENTs, "ParentID", "FullName", student.ParentID);
                    return View(student);
                }

                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentID = new SelectList(db.PARENTs, "ParentID", "FullName", student.ParentID);
            return View(student);
        }

        // GET: STUDENT/Delete/5
        // GET: STUDENT/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            STUDENT student = db.STUDENTs.Find(id);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: STUDENT/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            STUDENT student = db.STUDENTs.Find(id);
            db.STUDENTs.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EnrolledClasses()
        {
            // Lấy StudentID từ session hoặc User.Identity
            int studentId = (int)Session["StudentID"];

            // Lấy danh sách lớp học mà học viên đã đăng ký
            var enrolledClasses = db.ENROLLMENTs
                .Where(e => e.StudentID == studentId)
                .Select(e => e.CLASS)
                .ToList();

            return View(enrolledClasses);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
