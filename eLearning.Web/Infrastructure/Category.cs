using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<UsersInCategory> UsersInCategory { get; set; }
        public virtual ICollection<LocalizedCategory> LocalizedCategories { get; set; }
    }
}