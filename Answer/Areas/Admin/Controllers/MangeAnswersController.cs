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
    public class MangeAnswersController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Admin/MangeAnswers/

        public ActionResult Index()
        {
            return View(db.Answers.ToList());
        }

        //
        // GET: /Admin/MangeAnswers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AnswerModels answermodels = db.Answers.Find(id);
            if (answermodels == null)
            {
                return HttpNotFound();
            }
            return View(answermodels);
        }

        //
        // POST: /Admin/MangeAnswers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AnswerModels answermodels = db.Answers.Find(id);
            db.DeleteAnswer(answermodels);
            //answermodels.User.Answers.Remove(answermodels);
            //db.Answers.Remove(answermodels);
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