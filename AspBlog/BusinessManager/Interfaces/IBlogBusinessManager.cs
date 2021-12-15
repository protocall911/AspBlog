using System.Security.Claims;
using System.Threading.Tasks;
using AspBlog.Data.DataModels;
using AspBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspBlog.BusinessManager.Interfaces
{
    public interface IBlogBusinessManager
    {
        Task<Blog> CreateBlog(CreateBlogViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditBlogViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);

        Task<ActionResult<EditBlogViewModel>> UpdateBlog(EditBlogViewModel editBlogViewModel,
            ClaimsPrincipal claimsPrincipal);  
    }
}