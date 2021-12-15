    using System.Security.Claims;
    using System.Threading.Tasks;
    using AspBlog.Models.AdminViewModels;

    namespace AspBlog.BusinessManager.Interfaces
    {
        public interface IAdminBusinessManager
        {
            Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal);
        }
    }