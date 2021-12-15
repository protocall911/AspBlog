using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AspBlog.Areas.Identity.Data;
using AspBlog.Auth;
using AspBlog.BusinessManager.Interfaces;
using AspBlog.Data.DataModels;
using AspBlog.Models.BlogViewModels;
using AspBlog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspBlog.BusinessManager
{
    public class BlogBusinessManager : IBlogBusinessManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IBlogServices _blogServices;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public BlogBusinessManager(UserManager<ApplicationUser> userManager, IBlogServices blogServices,
            IWebHostEnvironment webHostEnvironment, IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            _blogServices = blogServices;
            _webHostEnvironment = webHostEnvironment;
            _authorizationService = authorizationService;
        }
        public async Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal)
        {
            Blog blog = createBlogViewModel.Blog;

            blog.Creator = await userManager.GetUserAsync(claimsPrincipal);
            
            blog.UpdatedOn = DateTime.Now;
            
            blog = await _blogServices.Add(blog);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}/UserFiles/Blogs/{blog.Id}/HeaderImage.jpg";
            
            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createBlogViewModel.BlogHeaderImage.CopyToAsync(fileStream);
            }

            return blog;
            
        }

        public async Task<ActionResult<EditBlogViewModel>> UpdateBlog(EditBlogViewModel editBlogViewModel,
            ClaimsPrincipal claimsPrincipal)
        {
            var blog = _blogServices.GetBlog(editBlogViewModel.Blog.Id);
            if (blog is null)
            {
                return new NotFoundResult();
            }
            
            var authorizationResult = await _authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            blog.Published = editBlogViewModel.Blog.Published;
            blog.Title = editBlogViewModel.Blog.Title;
            blog.Content = editBlogViewModel.Blog.Content;
            blog.UpdatedOn = DateTime.UtcNow;

            if (editBlogViewModel.BlogHeaderImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}/UserFiles/Blogs/{blog.Id}/HeaderImage.jpg";
                
                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editBlogViewModel.BlogHeaderImage.CopyToAsync(fileStream);
                }
            }

            return new EditBlogViewModel
            {
                Blog = await _blogServices.Update(blog)
            };
        }

        public async Task<ActionResult<EditBlogViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
            {   
                return new BadRequestResult();
            }

            var blogId = id.Value;
            var blog = _blogServices.GetBlog(blogId);

            if (blog is null)
            {
                return new NotFoundResult();
            }

            var authorizationResult =
                await _authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);
            if (!authorizationResult.Succeeded)
            {
                return DetermineActionResult(claimsPrincipal);
            }

            return new EditBlogViewModel()
            {
                Blog = blog
            };
        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            } else return new ChallengeResult();
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}




















