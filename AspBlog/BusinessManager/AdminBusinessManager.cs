using System.Security.Claims;
using System.Threading.Tasks;
using AspBlog.Areas.Identity.Data;
using AspBlog.BusinessManager.Interfaces;
using AspBlog.Models.AdminViewModels;
using AspBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AspBlog.BusinessManager
{
    public class AdminBusinessManager : IAdminBusinessManager
    {
        private UserManager<ApplicationUser> _userManager;
        private IBlogServices _blogServices;

        public AdminBusinessManager(UserManager<ApplicationUser> userManager, IBlogServices blogServices)
        {
            _userManager = userManager;
            _blogServices = blogServices;
        }

        public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel()
            {
                Blogs = _blogServices.GetBlogs(applicationUser)

            };
        }
    }
}