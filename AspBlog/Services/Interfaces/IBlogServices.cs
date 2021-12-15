using System.Collections.Generic;
using System.Threading.Tasks;
using AspBlog.Areas.Identity.Data;
using AspBlog.Data.DataModels;

namespace AspBlog.Services.Interfaces
{
    public interface IBlogServices
    {
        Blog GetBlog(int blogId);
        Task<Blog> Add(Blog blog);
        Task<Blog> Update(Blog blog);
        IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser);

    }
}