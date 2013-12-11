using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using edurate.Web.Infrastructure;
using edurate.Web.Models;
using WebMatrix.WebData;

namespace edurate.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private EdurateDb db = new EdurateDb();
        
        //
        // GET: /Dashboard/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Courses()
        {
            var instructorId = WebSecurity.CurrentUserId;
            var model = from c in db.Courses
                        where c.InstructorId == instructorId
                        select new CourseListViewModel()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            StudentNumber = c.Students.Count,
                            Rating = c.CourseRatings.Count != 0 ? c.CourseRatings.Sum(cr => cr.Rating) : 0,
                            CategoryName = c.Category.Name,
                            CreatedDate = c.CreatedDate,
                            LastModifiedDate = c.LastModifiedDate,
                            Published = c.Published
                        };
            return View(model);
        }

        
        public ActionResult Chapters(int id)
        {
            var model = db.Chapters.Where(c => c.CourseId == id);
            ViewBag.CourseId = id;

            return View(model);
        }


        public ActionResult Quizes(int id)
        {
            var model = db.Quizes.Where(c => c.CourseId == id);
            ViewBag.CourseId = id;
            
            return View(model);
        }

        public ActionResult Questions(int id)
        {
            var model = db.Questions.Where(q => q.QuizId == id);
            ViewBag.QuizId = id;
            return View(model);
        }

        public ActionResult Articles()
        {
            var userId = WebSecurity.CurrentUserId;
            var model = from a in db.Articles
                        where a.UserId == userId
                        select new ArticleListViewModel()
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Rating = a.ArticleRatings.Count != 0 ? a.ArticleRatings.Sum(cr => cr.Rating) : 0,
                            CategoryName = a.Category.Name,
                            CreatedDate = a.CreatedDate,
                            LastModifiedDate = a.LastModifiedDate
                        };

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CourseMenu()
        {
            var instructorId = WebSecurity.CurrentUserId;
            var model = from c in db.Courses
                        where c.InstructorId == instructorId
                        select new CourseListViewModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        };
            return PartialView(model);
        }


    }
}
