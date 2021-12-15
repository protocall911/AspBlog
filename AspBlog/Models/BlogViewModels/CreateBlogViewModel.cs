using System.ComponentModel.DataAnnotations;
using AspBlog.Data.DataModels;
using Microsoft.AspNetCore.Http;

namespace AspBlog.Models.BlogViewModels
{
    public class CreateBlogViewModel
    {
        [Required, Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public Blog Blog { get; set; }
    }
}