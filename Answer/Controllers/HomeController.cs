using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Answer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DBContext db = new DBContext();
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            ViewBag.unum = db.UserProfiles.Count();
            ViewBag.qnum = db.Questions.Count();
            ViewBag.anum = db.Answers.Count();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
