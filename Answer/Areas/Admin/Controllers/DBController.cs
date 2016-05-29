using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Answer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DBController : Controller
    {
        DBContext db = new DBContext();

        //
        // GET: /Admin/DB/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Drop(string returnUrl)
        {
            db.drop();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoadMajors(string returnUrl)
        {
            db.AddingMajor();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult LoadUsers(string returnUrl)
        {
            db.AddingUser();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
    }
}
