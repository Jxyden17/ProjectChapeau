using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }
    }
}
