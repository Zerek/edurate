using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace edurate.Web.Infrastructure
{
    public class LocalizedCategory
    {
        [Key, Column(Order=0)]
        public int CategoryId { get; set; }
        [Key, Column(Order = 1)]
        public string CultureName { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        
        public virtual Category Category { get; set; }
    }
}