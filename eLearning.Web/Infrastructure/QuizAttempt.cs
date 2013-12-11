using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        public int Attempt { get; set; }

        public float Mark { get; set; }

        public DateTime StartedTime { get; set; }

        public DateTime FinishedTime { get; set; }

        public virtual ICollection<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }
    }
}