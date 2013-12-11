using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool Published { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        //[Required]
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int InstructorId { get; set; }
        [ForeignKey("InstructorId")]
        public virtual User Instructor { get; set; }

        public virtual ICollection<User> Students { get; set; }

        public virtual ICollection<CourseRating> CourseRatings { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }

        public virtual ICollection<Quiz> Quizes { get; set; }
    }
}