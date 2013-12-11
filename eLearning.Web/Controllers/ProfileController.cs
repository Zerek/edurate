using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using edurate.Web.Infrastructure;
using edurate.Web.Models;
using WebMatrix.WebData;

namespace edurate.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        private EdurateDb db = new EdurateDb();

        public ActionResult Index()
        {
            var user = db.Users.Find(WebSecurity.CurrentUserId);

            return View(user);
        }


        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var user = db.Users.Find(id);
            return View();
        }

        #region Update Actions
        public ActionResult Edit()
        {
            var id = WebSecurity.CurrentUserId;
            var user = db.Users.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageUrl = user.ImageUrl;
            var model = EntityIntoViewModel(user);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel editedUser)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(editedUser.UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                if (editedUser.File != null)
                {
                    var image = new WebImage(editedUser.File.InputStream);
                    if (image != null)
                    {
                        var imageName = Guid.NewGuid().ToString() + "." + image.ImageFormat;
                        var path = Path.Combine(Server.MapPath("~/Content/Images"), WebSecurity.CurrentUserName, "Profile");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        path = Path.Combine(path, imageName);
                        image.Save(path);
                        user.ImageName = imageName;
                        user.ImageUrl = Path.Combine("~/Content/Images", WebSecurity.CurrentUserName, "Profile", imageName);
                    }
                }
                user.FullName = editedUser.FullName;
                user.Description = editedUser.Description;
                user.DateOfBirth = editedUser.DateOfBirth;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }

            return View(editedUser);
        }
        #endregion


        #region Unused Methods
        private User ViewModelIntoEntity(ProfileViewModel viewModel)
        {
            return new User()
                {
                    UserId = viewModel.UserId,
                    FullName = viewModel.FullName,
                    Description = viewModel.Description,
                    DateOfBirth = viewModel.DateOfBirth
                };
        }

        private ProfileViewModel EntityIntoViewModel(User entity)
        {
            return new ProfileViewModel()
                {
                    UserId = entity.UserId,
                    FullName = entity.FullName,
                    Description = entity.Description,
                    DateOfBirth = entity.DateOfBirth
                };
        }
        #endregion

    }
}
