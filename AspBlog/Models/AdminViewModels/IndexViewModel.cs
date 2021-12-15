using System.Collections.Generic;
using AspBlog.Data.DataModels;

namespace AspBlog.Models.AdminViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
    }
}