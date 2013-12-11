using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class UsersInCategory
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int Rating { get; set; }
    }
}