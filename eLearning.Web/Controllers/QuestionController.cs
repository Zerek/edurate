using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edurate.Web.Infrastructure;

namespace edurate.Web.Controllers
{
    public class QuestionController : Controller
    {
        private EdurateDb db = new EdurateDb();

        //
        // GET: /Question/

        public ActionResult List(int id, int amount = 5)
        {
            var questions = db.Questions.Where(q => q.QuizId == id);
            return View(questions.ToList());
        }

        //
        // GET: /Question/Details/5

        public ActionResult Details(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        #region Create Actions
        //
        // GET: /Question/Create

        public ActionResult Create(int id, QuestionType questionType)
        {
            ViewBag.QuizId = id;
            ViewBag.QuestionType = questionType;
            return PartialView();
        }

        //
        // POST: /Question/Create

        [HttpPost]
        public ActionResult Create(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Questions", "Dashboard", new { id = question.QuizId });
            }

            ViewBag.QuizId = new SelectList(db.Quizes, "Id", "Name", question.QuizId);
            return View(question);
        }

        public ActionResult AddAnswer(string index, QuestionType questionType)
        {
            ViewBag.Index = index;
            ViewBag.QuestionType = questionType;
            return PartialView();
        }
        #endregion

        #region Edit Actions
        //
        // GET: /Question/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuizId = new SelectList(db.Quizes, "Id", "Name", question.QuizId);
            return View(question);
        }

        //
        // POST: /Question/Edit/5

        [HttpPost]
        public ActionResult Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuizId = new SelectList(db.Quizes, "Id", "Name", question.QuizId);
            return View(question);
        }
        #endregion

        #region Delete Actions
        //
        // GET: /Question/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Question/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}