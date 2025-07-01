using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int categoryId);
    }
}
