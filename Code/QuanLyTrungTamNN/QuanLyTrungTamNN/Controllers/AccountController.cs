using Microsoft.Ajax.Utilities;
using QuanLyTrungTamNN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTrungTamNN.Controllers
{
    public class AccountController : Controller
    {
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu!";
                return View();
            }

            var user = db.USERs.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);
            if (user == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }

            // Lưu vào session
            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;
            Session["Role"] = user.ROLE.RoleName;

            // Nếu là học sinh, tìm StudentID
            if (user.ROLE.RoleName.ToLower() == "student")
            {
                var student = db.STUDENTs.FirstOrDefault(s => s.Email == user.Email);
                if (student != null)
                {
                    Session["StudentID"] = student.StudentID;
                }
                else
                {
                    ViewBag.Error = "Lỗi! Tài khoản này chưa được liên kết với học sinh nào.";
                    return View("Login");
                }
            }


            if (user.ROLE.RoleName.ToLower() == "teacher")
            {
                var teacher = db.TEACHERs.FirstOrDefault(t => t.Email == user.Email);
                if (teacher != null)
                {
                    Session["TeacherID"] = teacher.TeacherID;
                }
                else
                {
                    ViewBag.Error = "Lỗi! Tài khoản này chưa được liên kết với giáo viên nào.";
                    return View("Login");
                }
            }

            return RedirectToRoleDashboard(user.ROLE.RoleName);
        }

        private ActionResult RedirectToRoleDashboard(string roleName)
        {
            switch (roleName.ToLower())
            {
                case "admin":
                    return RedirectToAction("Dashboard", "Admin");
                case "teacher":
                    return RedirectToAction("MyClasses", "TEACHERs");
                case "student":
                    return RedirectToAction("Index", "STUDENTs");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}


