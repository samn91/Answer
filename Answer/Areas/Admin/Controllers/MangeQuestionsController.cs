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
    public class MangeQuestionsController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Admin/MangeQuestions/

        public ActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        //
        // GET: /Admin/MangeQuestions/Details/5

        public ActionResult Details(int id = 0)
        {
            QuestionModels Questions = db.Questions.Find(id);
            if (Questions == null)
            {
                return HttpNotFound();
            }
            return View(Questions);
        }

        //
        // GET: /Admin/MangeQuestions/Edit/5

        public ActionResult Edit(int id = 0)
        {
            QuestionModels Questions = db.Questions.Find(id);
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            if (Questions == null)
            {
                return HttpNotFound();
            }
            return View(Questions);
        }

        //
        // POST: /Admin/MangeQuestions/Edit/5

        [HttpPost]
        public ActionResult Edit(QuestionModels Questions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Questions).State = EntityState.Modified;
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
            return View(Questions);
        }

        //
        // GET: /Admin/MangeQuestions/Delete/5

        public ActionResult Delete(int id = 0)
        {
            QuestionModels Questions = db.Questions.Find(id);
            if (Questions == null)
            {
                return HttpNotFound();
            }
            return View(Questions);
        }

        //
        // POST: /Admin/MangeQuestions/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionModels questionmodels = db.Questions.Find(id);
            db.DeleteQuestion(questionmodels);
            //questionmodels.User.Answers.RemoveAll(a => a.Question == questionmodels);
            //for (int i = 0; i < questionmodels.Answers.Count; i++)
            //{
            //    var item = questionmodels.Answers[i];
            //    db.Answers.Remove(item);
            //}
            //questionmodels.Answers.Clear();
            //questionmodels.User.Questions.Remove(questionmodels);
            //db.Questions.Remove(questionmodels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateAnswer(AnswerModels a, int QId)
        {
            var q = db.Questions.Find(QId);
            a.Date = DateTime.Now;
            a.User = db.GetUser(User.Identity.Name);
            a.Question = q;
            q.Answers.Add(a);
            db.SaveChanges();
            return RedirectToAction("/Details/" + a.Question.QuestionId);
        }

        public ActionResult RemoveAnswer(int AId)
        {
            var a = db.Answers.Find(AId);
            int QId = a.Question.QuestionId;
            a.User.Answers.Remove(a);
            db.Answers.Remove(a);
            db.SaveChanges();
            return RedirectToAction("/Details/" + QId);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}