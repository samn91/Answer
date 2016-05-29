using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Answer.Models;

namespace Answer.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private DBContext db = new DBContext();

        //
        // GET: /Question/
        [AllowAnonymous]
        public ActionResult Index(int id = 1)
        {
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            ViewBag.IsCat = false;
            id--;
            if (id < 0)
                id = 0;
            int perpage = 4;
            int size = db.Questions.Count();
            if (size % perpage != 0)
                ViewBag.pages = (size / perpage) + 1;
            else
                ViewBag.pages = (size / perpage) ;
            var x = db.Questions
                //.Where(q => q.ReferredUser == null || q.ReferredUser.UserName == User.Identity.Name || q.User.UserName == User.Identity.Name)
                .OrderByDescending(o => o.Date)
                .Skip(id * perpage)
                .Take(perpage)
                .ToList();
            return View(x);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(int id = 1, int MajorId = 1)
        {
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString(),
                                                   Selected = MajorId == major.MajorId
                                               };
            ViewBag.Cat = List;
            ViewBag.IsCat = true;

            id--;
            if (id < 0)
                id = 0;
            int perpage = 4;
            int size = db.Questions.Count();
            if (size % perpage != 0)
                ViewBag.pages = (size / perpage) + 1;
            else
                ViewBag.pages = (size / perpage);
            if (MajorId == 1)
                return RedirectToAction("Index");
            var x = db.Questions
               .Where(q => q.Major.MajorId == MajorId)
               .OrderByDescending(o => o.Date)
               //.Skip(id * perpage)
               //.Take(perpage)
               .ToList();
            return View(x);
        }

        //
        // GET: /Question/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            QuestionModels questionmodels = db.Questions.Find(id);
            if (questionmodels == null)
            {
                return HttpNotFound();
            }
            questionmodels.Views++;
            db.SaveChanges();
            return View(questionmodels);
        }

        //
        // GET: /Question/Create

        public ActionResult Create()
        {
            DBContext db = new DBContext();
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o=>o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            ViewBag.Users = null;
            return View();
        }

        //
        // POST: /Question/Create

        [HttpPost]
        public ActionResult Create(QuestionModels questionmodels, string Type, int? ChoosenUserId)
        {
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o=>o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            if (ModelState.IsValid)
            {
                UserProfile user = db.GetUser(User.Identity.Name);
                UserProfile ChoosenUser = db.UserProfiles.Find(ChoosenUserId);
                questionmodels.Date = DateTime.Now;
                questionmodels.User = user;
                if (Type == "Paid")
                {
                    if(questionmodels.Major.MajorId == 1)
                        return View(questionmodels);
                    if (ChoosenUser == null)
                    {
                        var x = db.UserProfiles.Where(u => u.UserId != user.UserId && u.Major.MajorId == questionmodels.Major.MajorId).OrderByDescending(u => u.Rate).Take(10);
                        if (x.Count() == 0)
                            x = db.UserProfiles.Where(u => u.UserId != user.UserId).OrderByDescending(u => u.Rate).Take(10);
                        ViewBag.Users = x;
                        return View(questionmodels);
                    }
                    questionmodels.ReferredUser = ChoosenUser;
                    ChoosenUser.RefferdQuestions.Add(questionmodels);
                }
                else
                    questionmodels.ReferredUser = null;
                questionmodels.Major = db.Majors.Find(questionmodels.Major.MajorId);
                user.Questions.Add(questionmodels);
                db.Questions.Add(questionmodels);
                db.SaveChanges();
                return RedirectToAction("Details/" + questionmodels.QuestionId);
            }
            return View(questionmodels);
        }

        //
        // GET: /Question/Edit/5

        public ActionResult Edit(int id = 0)
        {
            QuestionModels questionmodels = db.Questions.Find(id);
            if (questionmodels == null)
            {
                return HttpNotFound();
            }
            if(questionmodels.User.UserName!=User.Identity.Name)
                return RedirectToAction("Details/"+id);
            if (questionmodels.ReferredUser != null)
            {
                var x = db.UserProfiles.Where(u => u.UserName != User.Identity.Name && u.Major.MajorId == questionmodels.Major.MajorId).OrderByDescending(u => u.Rate).Take(10);
                if (x.Count() == 0)
                    x = db.UserProfiles.Where(u => u.UserName != User.Identity.Name).OrderByDescending(u => u.Rate).Take(10);
                ViewBag.Users = x;
            }
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o=>o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            return View(questionmodels);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        public ActionResult Edit(QuestionModels questionmodels, string Type, int? ChoosenUserId)
        {
            IEnumerable<SelectListItem> List = from major in db.Majors.OrderBy(o => o.MajorType).ToList()
                                               select new SelectListItem
                                               {
                                                   Value = major.MajorId.ToString(),
                                                   Text = major.MajorType.ToString()
                                               };
            ViewBag.Cat = List;
            //if (ModelState.IsValid)
            //{
            UserProfile ChoosenUser = db.UserProfiles.Find(ChoosenUserId);
            var oq = db.Questions.Find(questionmodels.QuestionId);
            if (Type == "Paid")
            {
                if (questionmodels.Major.MajorId == 1)
                    return View(questionmodels);
                if (ChoosenUser == null)
                {
                    var x = db.UserProfiles.Where(u => u.UserName != User.Identity.Name && u.Major.MajorId == questionmodels.Major.MajorId).OrderByDescending(u => u.Rate).Take(10);
                    if (x.Count() == 0)
                        x = db.UserProfiles.Where(u => u.UserName != User.Identity.Name).OrderByDescending(u => u.Rate).Take(10);
                    ViewBag.Users = x;
                    return View(questionmodels);
                }
                ChoosenUser.RefferdQuestions.Add(questionmodels);
            }
            else
            {
                if (oq.ReferredUser != null)
                {
                    oq.ReferredUser.RefferdQuestions.Remove(questionmodels);
                }
            }
            questionmodels.ReferredUser = ChoosenUser;
            oq.Major = db.Majors.Find(questionmodels.Major.MajorId);
            oq.ReferredUser = questionmodels.ReferredUser;
            oq.Text = questionmodels.Text;
            oq.Title = questionmodels.Title;

            db.SaveChanges();
            return RedirectToAction("/Details/" + questionmodels.QuestionId);
            //}
            //return View(questionmodels);
        }

        //
        // GET: /Question/Delete/5

        public ActionResult Delete(int id = 0)
        {
            QuestionModels questionmodels = db.Questions.Find(id);
            if (questionmodels.User.UserName != User.Identity.Name)
                return RedirectToAction("Index");
            if (questionmodels == null)
            {
                return HttpNotFound();
            }
            return View(questionmodels);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionModels questionmodels = db.Questions.Find(id);
            questionmodels.User.Answers.RemoveAll(a => a.Question == questionmodels);
            for (int i = 0; i < questionmodels.Answers.Count; i++)
            {
                var item = questionmodels.Answers[i];
                db.Answers.Remove(item);
            }
            questionmodels.Answers.Clear();
            questionmodels.User.Questions.Remove(questionmodels);
            db.Questions.Remove(questionmodels);
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
            if (q.ReferredUser == null) //Free Question
                q.Answers.Add(a);
            else if (q.ReferredUser == a.User || q.User == a.User) //Paid Question
            {
                if (q.Answers.Count(an => an.User == q.ReferredUser) == 1)
                    if (q.User.Balance >= q.ReferredUser.Salary)
                    {
                        q.User.Balance -= q.ReferredUser.Salary;
                        q.ReferredUser.Balance += q.ReferredUser.Salary;
                        q.Answers.Add(a);
                    }
            }
            a.User.Answers.Add(a);
            db.SaveChanges();
            return RedirectToAction("/Details/" + a.Question.QuestionId);
        }

        public ActionResult RemoveAnswer( int AId)
        {
            var a = db.Answers.Find(AId);
            int QId = a.Question.QuestionId;
            if (a.User.UserName == User.Identity.Name)
            {
                db.Answers.Remove(a);
                db.SaveChanges();
            }
            return RedirectToAction("/Details/" + QId);
        }

        [HttpPost]
        public ActionResult RateAnswer(int QId,int AId)
        {
            var a = db.Answers.Find(AId);
            a.Rate.Add(new RateModels(a, db.GetUser(User.Identity.Name)));
            a.User.Rate++;
            db.SaveChanges();
            return RedirectToAction("/Details/" + a.Question.QuestionId);
        }

        [AllowAnonymous]
        public ActionResult Search(int id = 1, string Search = "")
        {
            id--;
            if (id < 0)
                id = 0;
            int perpage = 4;
            var ques = db.Questions.Where(q => q.Title.Contains(Search) || q.Text.Contains(Search));
            int size = ques.Count();
            if (size % perpage != 0)
                ViewBag.pages = (size / perpage) + 1;
            else
                ViewBag.pages = (size / perpage);
            ques = ques.OrderByDescending(o => o.Date).Skip(id * perpage).Take(perpage);
            ViewBag.Search = Search;
            return View(ques.ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Search(string Search = "")
        {
            int perpage = 4;
            var ques = db.Questions.Where(q => q.Title.Contains(Search) || q.Text.Contains(Search));
            int size = ques.Count();
            if (size % perpage != 0)
                ViewBag.pages = (size / perpage) + 1;
            else
                ViewBag.pages = (size / perpage);
            ViewBag.Search = Search;
            ques = ques.OrderByDescending(o => o.Date).Take(perpage);
            return View(ques.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}