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
        private TrungTamNgoaiNguEntities1 db = new TrungTamNgoaiNguEntities1(); // Đảm bảo đúng DbContext

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập tài khoản và mật khẩu!";
                return View();
            }

            // Tìm user trong database
            var user = db.USERs.FirstOrDefault(u => u.Username == username);

            if (user == null || user.PasswordHash != password)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }

            // Kiểm tra xem user có vai trò không
            if (user.ROLE == null || string.IsNullOrEmpty(user.ROLE.RoleName))
            {
                ViewBag.Error = "Tài khoản chưa được phân quyền!";
                return View();
            }

            // Lưu thông tin vào Session
            Session["UserID"] = user.UserID;
            Session["Username"] = user.Username;
            Session["Role"] = user.ROLE.RoleName;

            // Chuyển hướng dựa trên vai trò (không phân biệt hoa thường)
            switch (user.ROLE.RoleName.ToLower())
            {
                case "admin":
                    return RedirectToAction("Dashboard", "Admin");
                case "teacher":
                    return RedirectToAction("Dashboard", "Teacher");
                case "student":
                    return RedirectToAction("Dashboard", "Student");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        // Đăng xuất
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}