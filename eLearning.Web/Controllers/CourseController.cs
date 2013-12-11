using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using edurate.Web.Filters;
using edurate.Web.Infrastructure;
using edurate.Web.Models;
using WebMatrix.WebData;

namespace edurate.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private EdurateDb db = new EdurateDb();
        private DataManagement dataManagement = new DataManagement();
        private static readonly int _UprateValue = 1;
        private static readonly int _DownrateValue = -1;

        //
        // GET: /Course/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var courses = from c in db.Courses
                          where c.Published
                          select new CourseListViewModel()
                          {
                              Id = c.Id,
                              Name = c.Name,
                              CategoryName = c.Category.Name,
                              UserName = c.Instructor.Email,
                              Rating = c.CourseRatings.Count != 0 ? c.CourseRatings.Sum(cr => cr.Rating) : 0,
                              Image = c.ImageName
                          };
            
            return View(courses.ToList());
        }

        //
        // GET: /Course/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Rating = course.CourseRatings.Count != 0 ? course.CourseRatings.Sum(cr => cr.Rating) : 0;
            ViewBag.HasVoted = false;
            ViewBag.HasEnrolled = false;
            if (Request.IsAuthenticated)
            {
                var userId = WebSecurity.CurrentUserId;
                ViewBag.HasVoted = course.CourseRatings.Any(cr => cr.CourseId == course.Id 
                    && cr.UserId == userId);
                ViewBag.HasEnrolled = course.Students.Any(s => s.UserId == userId);
            }
            return View(course);
        }


        public ActionResult Enroll(int id = 0)
        {
            var course = db.Courses.Find(id);
            var userId = WebSecurity.CurrentUserId;
            var student = db.Users.Find(userId);

            if (course == null || student == null)
            {
                return HttpNotFound();
            }

            course.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = course.Id });
        }

        public ActionResult Class(int id = 0)
        {
            var course = db.Courses.Find(id);

            if (course == null)
            {
                return HttpNotFound();
            }

            var isEnrolled = course.Students.Any(s => s.UserId == WebSecurity.CurrentUserId);

            if (!isEnrolled)
            {
                return RedirectToAction("Details", id);
            }

            return View(course);
        }

        #region Status Update Actions

        [HttpPost]
        public ActionResult Uprate(int courseId, int instructorId, int categoryId)
        {
            var userId = WebSecurity.CurrentUserId;
            var courseRating = db.CourseRatings.Find(courseId, userId);

            if (courseRating == null)
            {
                db.CourseRatings.Add(new CourseRating()
                                        {
                                            CourseId = courseId,
                                            Rating = _UprateValue,
                                            UserId = userId
                                        });
                db.SaveChanges();

                dataManagement.UserRatingRepository.AddUserRating(instructorId, categoryId, _UprateValue);
            }
            else
            {
                if (courseRating.Rating == _DownrateValue)
                {
                    courseRating.Rating = _UprateValue;
                    db.SaveChanges();
                    dataManagement.UserRatingRepository.AddUserRating(instructorId, categoryId, _UprateValue);
                }
                
            }

            return RedirectToAction("Details", new { id = courseId });
        }
        
        [HttpPost]
        public ActionResult Downrate(int courseId, int instructorId, int categoryId)
        {
            var userId = WebSecurity.CurrentUserId;
            var courseRating = db.CourseRatings.Find(courseId, userId);

            if (courseRating == null)
            {
                db.CourseRatings.Add(new CourseRating()
                {
                    CourseId = courseId,
                    Rating = _DownrateValue,
                    UserId = userId
                });

                db.SaveChanges();

                dataManagement.UserRatingRepository.AddUserRating(instructorId, categoryId, _DownrateValue);
            }
            else
            {
                if (courseRating.Rating == _UprateValue)
                {
                    courseRating.Rating = _DownrateValue;
                    db.SaveChanges();
                    dataManagement.UserRatingRepository.AddUserRating(instructorId, categoryId, _DownrateValue);
                }

            }

            return RedirectToAction("Details", new { id = courseId });
        }

        [HttpPost]
        public ActionResult Publish(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Published = true;
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Courses", "Dashboard");
        }

        [HttpPost]
        public ActionResult Unpublish(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            course.Published = false;
            db.Entry(course).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Courses", "Dashboard");
        }
        #endregion

        #region Create Actions
        //
        // GET: /Course/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.InstructorId = WebSecurity.CurrentUserId;
            return View();
        }

        //
        // POST: /Course/Create
        
        [HttpPost]
        public ActionResult Create(CoursePostViewModel course)
        {
            
            if (ModelState.IsValid)
            {
                //saving image
                var image = new WebImage(course.File.InputStream);
                var imageName = Guid.NewGuid().ToString() + "." + image.ImageFormat;
                var path = Path.Combine(Server.MapPath("~/Content/Images"), WebSecurity.CurrentUserName, "Courses");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, imageName);
                image.Save(path);

                //saving course
                var dbCourse = ViewModelIntoEntity(course);
                dbCourse.CreatedDate = DateTime.Now;
                dbCourse.LastModifiedDate = DateTime.Now;
                dbCourse.Published = false;
                dbCourse.ImageName = imageName;
                dbCourse.ImageUrl = Path.Combine("~/Content/Images", WebSecurity.CurrentUserName, "Courses", imageName);
                db.Courses.Add(dbCourse);
                db.SaveChanges();

                
                return RedirectToAction("Courses", "Dashboard");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", course.CategoryId);
            ViewBag.InstructorId = WebSecurity.CurrentUserId;
            return View(course);
        }

        #endregion

        #region Edit Actions
        //
        // GET: /Course/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Course course = db.Courses.Find(id);
            //checks if course exists and if the current user is instroctor of the course
            if (course == null || course.InstructorId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            ViewBag.ImageUrl = course.ImageUrl;
            var model = EntityIntoViewModel(course);

            return View(model);
        }

        //
        // POST: /Course/Edit/5

        [HttpPost]
        public ActionResult Edit(EditedCourseViewModel updatedCourse)
        {
            
            if (ModelState.IsValid)
            {
                var course = db.Courses.Find(updatedCourse.Id);
               
                if (updatedCourse.File != null)
                {
                    var image = new WebImage(updatedCourse.File.InputStream);
                 
                    var imageName = Guid.NewGuid().ToString() + "." + image.ImageFormat;
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), WebSecurity.CurrentUserName, "Courses");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = Path.Combine(path, imageName);
                    image.Save(path);
                    course.ImageName = imageName;
                    course.ImageUrl = Path.Combine("~/Content/Images", WebSecurity.CurrentUserName, "Courses", imageName);
                }
                
                course.Name = updatedCourse.Name;
                course.Description = updatedCourse.Description;
                course.LastModifiedDate = DateTime.Now;
                
                db.SaveChanges();
                return RedirectToAction("Courses", "Dashboard");
            }
            return View(updatedCourse);
        }
        #endregion

        #region Delete Actions
        //
        // GET: /Course/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Course course = db.Courses.Find(id);
            if (course == null || course.InstructorId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Courses", "Dashboard");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private Course ViewModelIntoEntity(CoursePostViewModel viewModel)
        {
            return new Course()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId,
                    InstructorId = viewModel.InstructorId
                };
        }

        private EditedCourseViewModel EntityIntoViewModel(Course entity)
        {
            return new EditedCourseViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    Published = entity.Published
                };
        }

    }
}