using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public interface IAcademyDataSource
    {
        IQueryable<Course> Courses { get; }
        IQueryable<User> Users { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Chapter> Chapters { get; }
    }
}