using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjView.Models;

namespace prjView.Controllers
{
    public class HelperSampleController : Controller
    {
        // GET: HelperSample
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Member member)
        {
            ViewBag.UserID = member.UserId;
            ViewBag.Pwd = member.Pwd;
            ViewBag.Name = member.Name;
            ViewBag.Email = member.Email;
            ViewBag.Birthday = member.Birthday.ToShortDateString();

            return View();
        }
        public ActionResult Btn(Member member)
        {
            ViewBag.UserID = member.UserId;
            ViewBag.Pwd = member.Pwd;
            ViewBag.Name = member.Name;
            ViewBag.Email = member.Email;
            ViewBag.Birthday = member.Birthday.ToShortDateString();
            return View();
        }
    }
}