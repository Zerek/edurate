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
    [Authorize]
    public class ChapterController : Controller
    {
        private EdurateDb db = new EdurateDb();

        //
        // GET: /Chapter/

        public ActionResult List(int id)
        {
            var chapters = db.Chapters.Where(c => c.CourseId == id && c.Published).OrderBy(c=>c.Order).Select(c => new ChapterViewModel(){ Id = c.Id, Title = c.Title });
            return PartialView(chapters.ToList());
        }

        //
        // GET: /Chapter/Details/5

        public ActionResult Details(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return PartialView(chapter);
        }

        //
        // GET: /Chapter/Create

        public ActionResult Create(int id)
        {
            ViewBag.ParentId = new SelectList(db.Chapters.Where(c=>c.CourseId == id), "Id", "Title");
            ViewBag.CourseId = id;
            return View();
        }

        

        //
        // POST: /Chapter/Create

        [HttpPost]
        public ActionResult Create(Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                chapter.CreatedDate = DateTime.Now;
                chapter.LastModifiedDate = DateTime.Now;
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Chapters", "Dashboard", new { id = chapter.CourseId});
            }

            ViewBag.ParentId = new SelectList(db.Chapters.Where(c=>c.CourseId == chapter.CourseId), "Id", "Title", chapter.ParentId);
            ViewBag.CourseId = chapter.CourseId;
            return View(chapter);
        }

        //
        // GET: /Chapter/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null || chapter.Course.InstructorId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Chapters, "Id", "Title", chapter.ParentId);
            
            return View(chapter);
        }

        //
        // POST: /Chapter/Edit/5

        [HttpPost]
        public ActionResult Edit(Chapter updatedChapter)
        {
            if (ModelState.IsValid)
            {
                var chapter = db.Chapters.Find(updatedChapter.Id);

                chapter.Content = updatedChapter.Content;
                chapter.Order = updatedChapter.Order;
                chapter.Title = updatedChapter.Title;
                chapter.ParentId = updatedChapter.ParentId;
                //chapter.Published = updatedChapter.Published;
                chapter.LastModifiedDate = DateTime.Now;
                db.SaveChanges();
                
                return RedirectToAction("Chapters", "Dashboard", new { id = updatedChapter.CourseId });
            }
            ViewBag.ParentId = new SelectList(db.Chapters, "Id", "Title", updatedChapter.ParentId);
            
            return View(updatedChapter);
        }

        [HttpPost]
        public ActionResult Publish(int id)
        {
            var chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            chapter.Published = true;
            db.Entry(chapter).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Chapters", "Dashboard", new { id = chapter.CourseId });
        }

        [HttpPost]
        public ActionResult Unpublish(int id)
        {
            var chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            chapter.Published = false;
            db.Entry(chapter).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Chapters", "Dashboard", new { id = chapter.CourseId });
        }
        //
        // GET: /Chapter/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null || chapter.Course.InstructorId != WebSecurity.GetUserId(User.Identity.Name))
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        //
        // POST: /Chapter/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            var courseId = chapter.CourseId;
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("Chapters", "Dashboard", new { id = courseId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}