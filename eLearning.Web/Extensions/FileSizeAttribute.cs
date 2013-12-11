using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace edurate.Web.Extensions
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private int size = int.MaxValue;
        public FileSizeAttribute(int size)
        {
            this.size = size;
        }
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            //should be validated by RequiredAttribute
            if (file == null)
            {
                return true;
            }

            if (file.ContentLength < size)
            {
                return true;
            }

            return false;
        }
    }
}