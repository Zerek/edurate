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
    public class ArticleController : Controller
    {
        private EdurateDb db = new EdurateDb();
        private readonly int _ArticleValue = 10;
        private readonly int _UprateValue = 1;
        private readonly int _DownrateValue = -1;
        private DataManagement dataManagement = new DataManagement();

        //
        // GET: /Article/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.Category).Include(a => a.User);
            return View(articles.ToList());
        }

        //
        // GET: /Article/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        #region Update Actions

        [HttpPost]
        public ActionResult Uprate(int articleId, int userId, int categoryId)
        {
            
            var artivleRating = db.ArticleRatings.Find(articleId, WebSecurity.CurrentUserId);

            if (artivleRating == null)
            {
                db.ArticleRatings.Add(new ArticleRating()
                {
                    ArticleId = articleId,
                    Rating = _UprateValue,
                    UserId = userId
                });
                
                db.SaveChanges();
                dataManagement.UserRatingRepository.AddUserRating(userId, categoryId, _UprateValue);
            }
            else if (artivleRating.Rating == _DownrateValue)
            {
                artivleRating.Rating = _UprateValue;
                db.SaveChanges();
                dataManagement.UserRatingRepository.AddUserRating(userId, categoryId, _UprateValue);
            }
            
            return RedirectToAction("Details", new { id = articleId });
        }

        [HttpPost]
        public ActionResult Downrate(int articleId, int userId, int categoryId)
        {
            var artivleRating = db.ArticleRatings.Find(articleId, WebSecurity.CurrentUserId);

            if (artivleRating == null)
            {
                db.ArticleRatings.Add(new ArticleRating()
                {
                    ArticleId = articleId,
                    Rating = _DownrateValue,
                    UserId = userId
                });
                db.SaveChanges();
                dataManagement.UserRatingRepository.AddUserRating(userId, categoryId, _DownrateValue);
            }
            else if (artivleRating.Rating == _UprateValue)
            {
                artivleRating.Rating = _DownrateValue;
                db.SaveChanges();
                dataManagement.UserRatingRepository.AddUserRating(userId, categoryId, _DownrateValue);
            }

            return RedirectToAction("Details", new { id = articleId });
        }

        #endregion

        #region Create Actions
        //
        // GET: /Article/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.UserId = WebSecurity.CurrentUserId;
            return View();
        }

        //
        // POST: /Article/Create

        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreatedDate = DateTime.Now;
                article.LastModifiedDate = DateTime.Now;
                db.Articles.Add(article);
                db.SaveChanges();

                dataManagement.UserRatingRepository.AddUserRating(article.UserId, article.CategoryId, _ArticleValue);

                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            ViewBag.UserId = WebSecurity.CurrentUserId;
            return View(article);
        }

        #endregion

        #region Edit Actions
        //
        // GET: /Article/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null || article.UserId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }

        //
        // POST: /Article/Edit/5

        [HttpPost]
        public ActionResult Edit(Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", article.CategoryId);
            return View(article);
        }
        #endregion

        #region Delete Actions
        //
        // GET: /Article/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Article article = db.Articles.Find(id);
            if (article == null || article.UserId != WebSecurity.CurrentUserId)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        //
        // POST: /Article/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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