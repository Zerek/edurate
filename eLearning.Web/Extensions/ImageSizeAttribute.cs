using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace edurate.Web.Extensions
{
    public class ImageSizeAttribute : ValidationAttribute
    {

        private int width;
        private int height;

        public ImageSizeAttribute(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override bool IsValid(object value)
        {
            //should be checked by RequiredAttribute
            if (value == null)
            {
                return true;
            }
            
            var file = value as HttpPostedFileBase;
            
            if (file != null)
            {
                try
                {
                    var image = new Bitmap(file.InputStream);
                    if ((image != null) && (image.Width == width) && (image.Height == height))
                    {
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, new object[] { name, width, height });
        }
    }
}