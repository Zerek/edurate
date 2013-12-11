using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using edurate.Web.Infrastructure;

namespace edurate.Web.Models
{
    public class UserRatingRepository
    {

        public void AddUserRating(int userId, int categoryId, int value)
        {
            using (var db = new EdurateDb())
            {
                var userRating = db.UsersInCategory.Find(userId, categoryId);
                if (userRating == null)
                {
                    userRating = new UsersInCategory()
                    {
                        UserId = userId,
                        CategoryId = categoryId,
                        Rating = value
                    };
                    db.UsersInCategory.Add(userRating);
                    db.SaveChanges();
                }
                else
                {
                    userRating.Rating += value;
                    db.SaveChanges();
                }
            }
        }
    }
}