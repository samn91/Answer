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
    public class MangeMajorsController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Admin/MangeMajors/

        public ActionResult Index()
        {
            return View(db.Majors.OrderBy(m=>m.MajorType).ToList());
        }

        //
        // GET: /Admin/MangeMajors/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/MangeMajors/Create

        [HttpPost]
        public ActionResult Create(MajorModels majormodels)
        {
            if (ModelState.IsValid)
            {
                db.Majors.Add(majormodels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(majormodels);
        }

        //
        // GET: /Admin/MangeMajors/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MajorModels majormodels = db.Majors.Find(id);
            if (majormodels == null)
            {
                return HttpNotFound();
            }
            return View(majormodels);
        }

        //
        // POST: /Admin/MangeMajors/Edit/5

        [HttpPost]
        public ActionResult Edit(MajorModels majormodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(majormodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(majormodels);
        }

        //
        // GET: /Admin/MangeMajors/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MajorModels majormodels = db.Majors.Find(id);
            if (majormodels == null)
            {
                return HttpNotFound();
            }
            return View(majormodels);
        }

        //
        // POST: /Admin/MangeMajors/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MajorModels majormodels = db.Majors.Find(id);
            db.DeleteMajor(majormodels);
            //var x = db.Questions.Where(q => q.Major == majormodels).ToList();
            //foreach (var q in x)
            //{
            //    db.Answers.ToList().RemoveAll(y=>y.Question==q);
            //}

            //db.Questions.ToList().RemoveAll(q => x.Contains(q));
            //db.Majors.Remove(majormodels);
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