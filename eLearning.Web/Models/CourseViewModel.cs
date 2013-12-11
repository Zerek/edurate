using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using edurate.Web.Extensions;
using edurate.Web.Resources;

namespace edurate.Web.Models
{
    public class CourseListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public int StudentNumber { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool Published { get; set; }
        public string Image { get; set; }
    }

    public class CoursePostViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Published { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int InstructorId { get; set; }

        [Required]
        [Display(Name = "Image")]
        [ExtendedFileExtensions]
        [FileSize(1048576, ErrorMessageResourceName = "FileSizeError", ErrorMessageResourceType = typeof(UIResource))]
        [ImageSize(300, 200, ErrorMessageResourceName = "ImageSizeError", ErrorMessageResourceType = typeof(UIResource))]
        public HttpPostedFileBase File { get; set; }
    }

    public class EditedCourseViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Published { get; set; }

        [Display(Name = "Image")]
        [ExtendedFileExtensions]
        [FileSize(1048576, ErrorMessageResourceName = "FileSizeError", ErrorMessageResourceType = typeof(UIResource))]
        [ImageSize(300, 200, ErrorMessageResourceName = "ImageSizeError", ErrorMessageResourceType = typeof(UIResource))]
        public HttpPostedFileBase File { get; set; }
    }
}