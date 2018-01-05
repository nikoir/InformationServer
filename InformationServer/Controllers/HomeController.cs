using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationServer.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Display()
        {
            return new FilePathResult("~/Views/Home/display.html", "text/html");
        }
        public ActionResult Edit()
        {
            return new FilePathResult("~/Views/Home/edit.html", "text/html");
        }
    }
}