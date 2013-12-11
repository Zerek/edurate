using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public class QuizAttemptAnswer
    {
        [Key, Column(Order=0)]
        public int QuizAttemptId { get; set; }
        public virtual QuizAttempt QuizAttempt { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public int? QuestionAnswerId { get; set; }
        public virtual QuestionAnswer QuestionAnswer { get; set; }

        public string QuestionAnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }
}