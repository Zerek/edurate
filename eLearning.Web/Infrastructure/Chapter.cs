using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public class Chapter
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public int Order { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool Published { get; set; }

        public int? ParentId { get; set; }
        public virtual Chapter Parent { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

    }
}