using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edurate.Web.Models
{
    public class RatingViewModel
    {
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public int CategoryId { get; set; }
        public int CurrentRating { get; set; }
        
    }
}