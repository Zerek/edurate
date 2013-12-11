using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class UsersInCourse
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public DateTime LastEnteredDate { get; set; }
    }
}