using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspBlog.Areas.Identity.Data;
using AspBlog.Data.DataModels;
using AspBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspBlog.Services
{
    public class BlogServices : IBlogServices
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogServices(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Blog> Add(Blog blog)
        {
            _applicationDbContext.Add(blog);
            await _applicationDbContext.SaveChangesAsync();

            return blog;
        }
        
        public async Task<Blog> Update(Blog blog)
        {
            _applicationDbContext.Update(blog);
            await _applicationDbContext.SaveChangesAsync();

            return blog;
        }

        public Blog GetBlog(int blogId)
        {
            return _applicationDbContext.Blogs.FirstOrDefault(blog => blog.Id == blogId);
        }

        public IEnumerable<Blog> GetBlogs(ApplicationUser applicationUser)
        {
            return _applicationDbContext.Blogs
                .Include(blog => blog.Creator)
                .Include(blog => blog.Approver)
                .Include(blog => blog.Posts)
                .Where(blog => blog.Creator == applicationUser);
        }
    }
}