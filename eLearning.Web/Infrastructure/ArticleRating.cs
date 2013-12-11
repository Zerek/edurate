using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class ArticleRating
    {
        [Key, Column(Order = 0)]
        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int Rating { get; set; }
    }
}