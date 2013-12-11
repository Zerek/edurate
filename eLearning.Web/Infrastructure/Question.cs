using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public enum QuestionType
    { 
        [Display(Name="Multiple Choice")]
        MultipleChoice = 1,
        [Display(Name = "Short Answer")]
        ShortAnswer = 2,
        [Display(Name = "Upload")]
        Upload = 3
    }

    public class Question
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public QuestionType QuestionType { get; set; }

        public string Feedback { get; set; }

        public virtual ICollection<QuestionAnswer> Answers { get; set; }
    }
}