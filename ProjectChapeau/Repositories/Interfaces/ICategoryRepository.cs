using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int categoryId);
    }
}
