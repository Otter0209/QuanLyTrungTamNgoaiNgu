﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTrungTamNN.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}