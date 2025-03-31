using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyTrungTamNN.Models;

namespace QuanLyTrungTamNN.Controllers
{
    public class TEACHERsController : Controller
    {
        private readonly TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        // GET: TEACHERs
        public ActionResult Index()
        {
            // Lấy danh sách giáo viên
            var teachers = db.TEACHERs.ToList();
            return View(teachers);
        }

        // GET: TEACHERs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TEACHER teacher = db.TEACHERs.Find(id);
            if (teacher == null)
                return HttpNotFound();

            return View(teacher);
        }

        // GET: TEACHERs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TEACHERs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherID,FullName,Email,PhoneNumber,Expertise")] TEACHER teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.TEACHERs.Add(teacher);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Bạn có thể ghi log lỗi vào file hoặc hệ thống logging của bạn
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }
            return View(teacher);
        }

        // GET: TEACHERs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TEACHER teacher = db.TEACHERs.Find(id);
            if (teacher == null)
                return HttpNotFound();

            return View(teacher);
        }

        // POST: TEACHERs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherID,FullName,Email,PhoneNumber,Expertise")] TEACHER teacher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật dữ liệu: " + ex.Message);
                }
            }
            return View(teacher);
        }

        // GET: TEACHERs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TEACHER teacher = db.TEACHERs.Find(id);
            if (teacher == null)
                return HttpNotFound();

            return View(teacher);
        }

        // POST: TEACHERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                TEACHER teacher = db.TEACHERs.Find(id);
                db.TEACHERs.Remove(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, hiển thị thông báo
                ModelState.AddModelError("", "Đã xảy ra lỗi khi xóa dữ liệu: " + ex.Message);
                return View();
            }
        }
        // GET: TEACHERs/Attendance
        public ActionResult Attendance()
        {
            // Giả sử TeacherID của giáo viên đang đăng nhập được lưu trong Session
            int teacherId = Convert.ToInt32(Session["TeacherID"] ?? "0");
            if (teacherId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp mà giáo viên phụ trách
            var myClasses = db.CLASSes.Where(c => c.TeacherID == teacherId).ToList();
            return View(myClasses);
        }
        // GET: TEACHERs/MarkAttendance?classId=5
        public ActionResult MarkAttendance(int? classId)
        {
            if (classId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Lấy lớp học có ClassID bằng classId
            var cls = db.CLASSes
                        .Include(c => c.ENROLLMENTs.Select(e => e.STUDENT))
                        .FirstOrDefault(c => c.ClassID == classId);
            if (cls == null)
                return HttpNotFound();

            return View(cls);
        }

        // GET: TEACHERs/MyStudents
        public ActionResult MyStudents()
        {
            // Giả sử TeacherID được lưu trong Session khi giáo viên đăng nhập
            int teacherId = Convert.ToInt32(Session["TeacherID"] ?? "0");
            if (teacherId == 0)
            {
                // Nếu không có TeacherID, chuyển hướng đến trang đăng nhập hoặc thông báo lỗi
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách học viên theo lớp do giáo viên phụ trách
            var myStudents = db.ENROLLMENTs
                                .Where(e => e.CLASS.TeacherID == teacherId)
                                .Select(e => e.STUDENT)
                                .Distinct()
                                .ToList();

            return View(myStudents);
        }
        // GET: TEACHERs/StudentList
        public ActionResult StudentList()
        {
            // Lấy TeacherID từ Session (bạn cần đảm bảo TeacherID đã được lưu khi giáo viên đăng nhập)
            int teacherId = Convert.ToInt32(Session["TeacherID"] ?? "0");
            if (teacherId == 0)
            {
                // Nếu không có TeacherID, chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp mà giáo viên phụ trách, bao gồm thông tin học viên
            var myClasses = db.CLASSes
                              .Include(c => c.ENROLLMENTs.Select(e => e.STUDENT))
                              .Where(c => c.TeacherID == teacherId)
                              .ToList();

            return View(myClasses);
        }
        // GET: TEACHERs/MyClasses
        public ActionResult MyClasses()
        {
            int teacherId = Convert.ToInt32(Session["TeacherID"] ?? "0");
            if (teacherId == 0)
            {
                // Nếu không có TeacherID, chuyển hướng đến trang đăng nhập hoặc thông báo lỗi.
                return RedirectToAction("Login", "Account");
            }

            // Lấy danh sách lớp mà giáo viên phụ trách
            var myClasses = db.CLASSes.Where(c => c.TeacherID == teacherId).ToList();
            return View(myClasses);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
