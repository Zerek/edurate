using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edurate.Web.Infrastructure;
using edurate.Web.Models;
using WebMatrix.WebData;

namespace edurate.Web.Controllers
{
    public class QuizController : Controller
    {
        private EdurateDb db = new EdurateDb();
        private DataManagement dataManagement = new DataManagement();
        //
        // GET: /Quiz/

        public ActionResult List(int id)
        {
            var quizes = db.Quizes.Where(q => q.CourseId == id && q.Published).OrderBy(q=>q.Order);
            return PartialView(quizes.ToList());
        }

        //
        // GET: /Quiz/Details/5

        public ActionResult Details(int id = 0)
        {
            Quiz quiz = db.Quizes.Find(id);
            
            if (quiz == null)
            {
                return HttpNotFound();
            }
            //check if all user has finished all attempts
            //if yes doesn't show the start button
            var userId = WebSecurity.CurrentUserId;
            ViewBag.QuizAttemptNumber = db.QuizAttempts.Count(qa => qa.QuizId == id && qa.UserId == userId);

            return PartialView(quiz);
        }

        public ActionResult StartQuiz(int id)
        {
            var quiz = db.Quizes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            var userId = WebSecurity.CurrentUserId;
            
            //check if all user has finished all attempts
            var quizAttemptCount = db.QuizAttempts.Count(qa => qa.QuizId == id && qa.UserId == userId);
            if (quiz.Attempts <= quizAttemptCount)
            {
                return HttpNotFound();
            }
            
            //creating new "quizAttempt", saving it in db
            //and sending back the "quizAttempt" id back to the view
            var quizAttempt = new QuizAttempt();
            db.QuizAttempts.Add(quizAttempt);
            quizAttempt.Attempt = ++quizAttemptCount;
            quizAttempt.QuizId = quiz.Id;
            quizAttempt.UserId = userId;
            quizAttempt.StartedTime = DateTime.Now;
            db.SaveChanges();
            ViewBag.QuizAttemptId = quizAttempt.Id;

            //TO-DO: create question list
            var model = quiz.Questions.OrderBy(q => Guid.NewGuid()).Take(quiz.QuestionAmount);

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult FinishQuiz(int quizAttemptId, IEnumerable<QuizAttemptAnswer> quizAttemptAnswers)
        {
            var userId = WebSecurity.CurrentUserId;
            var quizAttempt = db.QuizAttempts.Find(quizAttemptId);
            if (quizAttempt == null || quizAttempt.UserId != userId)
            {
                return HttpNotFound();
            }
            //TO-DO: calculate the mark of the quiz
            //TO-DO: save the answers to "QuizAttemptAnswers"
            var mark = 0;
            foreach (var quizAttemptAnswer in quizAttemptAnswers)
            {
                var question = db.Questions.Find(quizAttemptAnswer.QuestionId);
                quizAttemptAnswer.IsCorrect = false;
                if (question.QuestionType == QuestionType.MultipleChoice)
                {
                    var answer = db.QuestionAnswers.Find(quizAttemptAnswer.QuestionAnswerId);
                    if (answer.IsRight)
                    {
                        mark++;
                        quizAttemptAnswer.IsCorrect = true;
                    }
                }
                else if (question.QuestionType == QuestionType.ShortAnswer)
                {
                    foreach (var item in question.Answers)
                    {
                        if (item.Content.Equals(quizAttemptAnswer.QuestionAnswerText))
                        {
                            mark++;
                            quizAttemptAnswer.IsCorrect = true;
                            break;
                        }
                    }
                }
                quizAttemptAnswer.QuizAttemptId = quizAttemptId;
                db.QuizAttemptAnswers.Add(quizAttemptAnswer);
            }
            quizAttempt.FinishedTime = DateTime.Now;
            quizAttempt.Mark = mark;

            //increasing the user rating
            db.SaveChanges();

            var categoryId = quizAttempt.Quiz.Course.CategoryId;
            dataManagement.UserRatingRepository.AddUserRating(userId, categoryId, mark);
            //return PartialView("_QuizFeedback", quizAttempt);
            return RedirectToAction("Details", new { id = quizAttempt.QuizId });
        }

        [HttpPost]
        public ActionResult FeedbackQuiz(int id, int attempt)
        {
            var userId = WebSecurity.CurrentUserId;
            var quizAttempt = db.QuizAttempts.Where(qa => qa.UserId == userId && qa.QuizId == id && qa.Attempt == attempt).FirstOrDefault();
            if (quizAttempt == null)
            {
                return HttpNotFound();
            }
            
            return PartialView("_QuizFeedback", quizAttempt);
        }

        #region Update Status Actions
        //
        // POST: Quiz/Publish
        [HttpPost]
        public ActionResult Publish(int id)
        {
            var quiz = db.Quizes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            quiz.Published = true;
            db.Entry(quiz).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Quizes", "Dashboard", new { id = quiz.CourseId });
        }

        //
        // POST: Quiz/Unpublish
        [HttpPost]
        public ActionResult Unpublish(int id)
        {
            var quiz = db.Quizes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            quiz.Published = false;
            db.Entry(quiz).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Quizes", "Dashboard", new { id = quiz.CourseId });
        }
        #endregion

        #region Create Actions
        //
        // GET: /Quiz/Create/id

        public ActionResult Create(int id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        //
        // POST: /Quiz/Create

        [HttpPost]
        public ActionResult Create(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                quiz.CreatedDate = DateTime.Now;
                quiz.LastModifiedDate = DateTime.Now;
                db.Quizes.Add(quiz);
                db.SaveChanges();
                return RedirectToAction("Quizes", "Dashboard", new { id = quiz.CourseId});
            }

            ViewBag.CourseId = quiz.Id;
            return View(quiz);
        }
        #endregion

        #region Edit Actions
        //
        // GET: /Quiz/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Quiz quiz = db.Quizes.Find(id);
            if (quiz == null || quiz.Course.InstructorId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            
            return View(quiz);
        }

        //
        // POST: /Quiz/Edit/5

        [HttpPost]
        public ActionResult Edit(Quiz editedQuiz)
        {
            if (ModelState.IsValid)
            {
                var quiz = db.Quizes.Find(editedQuiz.Id);
                quiz.Name = editedQuiz.Name;
                quiz.Order = editedQuiz.Order;
                quiz.QuestionAmount = editedQuiz.QuestionAmount;
                quiz.Description = editedQuiz.Description;
                quiz.Attempts = editedQuiz.Attempts;
                quiz.LastModifiedDate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Quizes", "Dashboard", new { id = editedQuiz.Id});
            }
            
            return View(editedQuiz);
        }
        #endregion

        #region Delete Actions
        //
        // GET: /Quiz/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Quiz quiz = db.Quizes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        //
        // POST: /Quiz/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Quiz quiz = db.Quizes.Find(id);
            db.Quizes.Remove(quiz);
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