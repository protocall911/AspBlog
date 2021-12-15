using System;
using System.Threading.Tasks;
using AspBlog.BusinessManager.Interfaces;
using AspBlog.Data.DataModels;
using AspBlog.Models.BlogViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspBlog.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager blogBusinessManager;

        public BlogController(IBlogBusinessManager blogBusinessManager)
        {
            this.blogBusinessManager = blogBusinessManager;
        }
    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new CreateBlogViewModel());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = await blogBusinessManager.GetEditViewModel(id, User);

            if (result.Result is null)
            {
                return View(result.Value);
            }

            return result.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateBlogViewModel createBlogViewModel)
        {
            createBlogViewModel.Blog.UpdatedOn = DateTime.Now;
            await blogBusinessManager.CreateBlog(createBlogViewModel, User);
            return RedirectToAction("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditBlogViewModel editBlogViewModel)
        {
            editBlogViewModel.Blog.UpdatedOn = DateTime.Now;
            var actionResult = await blogBusinessManager.UpdateBlog(editBlogViewModel, User);
            if (actionResult.Result is null)
            {
                return RedirectToAction("Edit", new {editBlogViewModel.Blog.Id});
            }
            return actionResult.Result;
        }
    }
}















