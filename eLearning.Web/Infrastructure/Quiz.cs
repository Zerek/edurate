using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public class Quiz
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int Order { get; set; }
        
        [Required]
        public int Attempts { get; set; }

        [Required]
        public int QuestionAmount { get; set; }

        [Required]
        public bool Published { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}