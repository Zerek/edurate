using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Views { get; set; }

        public virtual ICollection<ArticleRating> ArticleRatings { get; set; }
    }
}