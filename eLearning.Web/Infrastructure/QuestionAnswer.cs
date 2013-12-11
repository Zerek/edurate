using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public class QuestionAnswer
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public string Feedback { get; set; }
        
        public bool IsRight { get; set; }
    }
}