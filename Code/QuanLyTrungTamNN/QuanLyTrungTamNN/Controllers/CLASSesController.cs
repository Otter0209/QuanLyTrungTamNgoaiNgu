using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyTrungTamNN.Models;

namespace QuanLyTrungTamNN.Controllers
{
    public class CLASSesController : Controller
    {
        // Khởi tạo DbContext với readonly (theo gợi ý của IDE)
        private readonly TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: CLASSes
        public ActionResult Index()
        {
            // Lấy danh sách lớp, include thông tin giáo viên chủ nhiệm
            var classes = db.CLASSes.Include(c => c.TEACHER).ToList();
            return View(classes);
        }

        // GET: CLASSes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CLASS cls = db.CLASSes.Find(id);
            if (cls == null)
                return HttpNotFound();

            return View(cls);
        }

        // GET: CLASSes/Create
        public ActionResult Create()
        {
            // Cung cấp danh sách giáo viên để chọn cho TeacherID
            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName");
            return View();
        }

        // POST: CLASSes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClassID,ClassName,Schedule,Location,TeacherID,ClassTime")] CLASS cls)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(cls.ClassName))
                ModelState.AddModelError("ClassName", "Vui lòng nhập tên lớp.");
            if (string.IsNullOrWhiteSpace(cls.Schedule))
                ModelState.AddModelError("Schedule", "Vui lòng nhập lịch học.");
            if (string.IsNullOrWhiteSpace(cls.Location))
                ModelState.AddModelError("Location", "Vui lòng nhập địa điểm.");
            if (cls.TeacherID <= 0)
                ModelState.AddModelError("TeacherID", "Vui lòng chọn giáo viên chủ nhiệm.");
            if (string.IsNullOrWhiteSpace(cls.ClassTime))
                ModelState.AddModelError("ClassTime", "Vui lòng nhập thời gian lớp học.");

            if (ModelState.IsValid)
            {
                try
                {
                    db.CLASSes.Add(cls);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi tạo lớp: " + ex.Message);
                }
            }

            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cls.TeacherID);
            return View(cls);
        }

        // GET: CLASSes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CLASS cls = db.CLASSes.Find(id);
            if (cls == null)
                return HttpNotFound();

            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cls.TeacherID);
            return View(cls);
        }

        // POST: CLASSes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClassID,ClassName,Schedule,Location,TeacherID,ClassTime")] CLASS cls)
        {
            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(cls.ClassName))
                ModelState.AddModelError("ClassName", "Vui lòng nhập tên lớp.");
            if (string.IsNullOrWhiteSpace(cls.Schedule))
                ModelState.AddModelError("Schedule", "Vui lòng nhập lịch học.");
            if (string.IsNullOrWhiteSpace(cls.Location))
                ModelState.AddModelError("Location", "Vui lòng nhập địa điểm.");
            if (cls.TeacherID <= 0)
                ModelState.AddModelError("TeacherID", "Vui lòng chọn giáo viên chủ nhiệm.");
            if (string.IsNullOrWhiteSpace(cls.ClassTime))
                ModelState.AddModelError("ClassTime", "Vui lòng nhập thời gian lớp học.");

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(cls).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật lớp: " + ex.Message);
                }
            }

            ViewBag.TeacherID = new SelectList(db.TEACHERs, "TeacherID", "FullName", cls.TeacherID);
            return View(cls);
        }

        // GET: CLASSes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CLASS cls = db.CLASSes.Find(id);
            if (cls == null)
                return HttpNotFound();

            return View(cls);
        }

        // POST: CLASSes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                CLASS cls = db.CLASSes.Find(id);
                db.CLASSes.Remove(cls);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi xóa lớp: " + ex.Message);
                return View();
            }
        }

        // GET: CLASSes/StudentList/5
        public ActionResult StudentList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Lấy lớp có ClassID bằng id và include các Enrollment với thông tin STUDENT
            var cls = db.CLASSes
                        .Include(c => c.ENROLLMENTs.Select(e => e.STUDENT))
                        .FirstOrDefault(c => c.ClassID == id);

            if (cls == null)
            {
                return HttpNotFound();
            }

            return View(cls);
        }

        [HttpPost]
        public JsonResult UpdateAttendance(int studentId, string status)
        {
            var student = db.STUDENTs.Find(studentId);
            if (student == null)
            {
                return Json(new { success = false, message = "Không tìm thấy học viên." });
            }

            // Cập nhật trạng thái điểm danh
            student.AttendanceStatus = status;
            db.SaveChanges();

            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
