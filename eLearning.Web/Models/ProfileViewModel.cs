using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using edurate.Web.Extensions;
using edurate.Web.Resources;

namespace edurate.Web.Models
{
    public class ProfileViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "FullNameDisplay", ResourceType = typeof(UIResource))]
        public string FullName { get; set; }
        
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "DescriptionDisplay", ResourceType = typeof(UIResource))]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="BirthdayDisplay", ResourceType = typeof(UIResource))]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "ImageDisplay", ResourceType = typeof(UIResource))]
        [ExtendedFileExtensions]
        [FileSize(524288, ErrorMessageResourceName="FileSizeError", ErrorMessageResourceType=typeof(UIResource))]
        [ImageSize(140, 140, ErrorMessageResourceName = "ImageSizeError", ErrorMessageResourceType = typeof(UIResource))]
        public HttpPostedFileBase File { get; set; }
    }


}