using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Answer.Models;

namespace Answer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MangeUsersController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Admin/MangeUsers/

        public ActionResult Index()
        {
            return View(db.UserProfiles.ToList());
        }

        //
        // GET: /Admin/MangeUsers/Details/5

        public ActionResult Details(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            ViewBag.refer = db.Questions.Where(q => q.User == userprofile);
            ViewBag.answers = db.Answers.Where(a => a.User == userprofile);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // GET: /Admin/MangeUsers/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/MangeUsers/Create

        [HttpPost]
        public ActionResult Create(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userprofile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userprofile);
        }

        //
        // GET: /Admin/MangeUsers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Admin/MangeUsers/Edit/5

        [HttpPost]
        public ActionResult Edit(UserProfile userprofile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userprofile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            return View(userprofile);
        }

        //
        // GET: /Admin/MangeUsers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            if (userprofile == null)
            {
                return HttpNotFound();
            }
            return View(userprofile);
        }

        //
        // POST: /Admin/MangeUsers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userprofile = db.UserProfiles.Find(id);
            db.DeleteUser(userprofile);
            //foreach (var q in userprofile.Questions)
            //{
            //    db.Answers.ToList().RemoveAll(x => q.Answers.Contains(x));
            //}
            //db.Questions.ToList().RemoveAll(q => userprofile.Questions.Contains(q));
            //db.UserProfiles.Remove(userprofile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}