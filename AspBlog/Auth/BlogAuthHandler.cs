using System.Threading.Tasks;
using AspBlog.Areas.Identity.Data;
using AspBlog.Data.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace AspBlog.Auth
{
    public class BlogAuthHandler : AuthorizationHandler<OperationAuthorizationRequirement, Blog>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogAuthHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager; 
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            Blog resource)
        {
            var applicationUser = await _userManager.GetUserAsync(context.User);

            if ((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator)
            {
                context.Succeed(requirement);
            }
        }
    }
}