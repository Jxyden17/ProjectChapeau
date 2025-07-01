using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.ViewComponents
{
    public class CategoryFilterViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryFilterViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            List <Category> categories = _categoryService.GetAllCategories();
            return View(categories);
        }
    }
}
