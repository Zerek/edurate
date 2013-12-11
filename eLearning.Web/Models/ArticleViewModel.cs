using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace edurate.Web.Models
{
    public class ArticleListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string CategoryName { get; set; }
    }
}