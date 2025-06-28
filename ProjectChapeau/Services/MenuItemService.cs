using AspNetCoreGeneratedDocument;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemsRepository;

        public MenuItemService(IMenuItemRepository menuItemsRepository)
        {
            _menuItemsRepository = menuItemsRepository;
        }
        
        public MenuItem? GetMenuItemById(int menuItemId)
        {
            return _menuItemsRepository.GetMenuItemById(menuItemId);
        }

        public List<MenuItem> GetCategory(int categoryId)
        {
            return _menuItemsRepository.GetCategory(categoryId);
        }

        public List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId)
        {
            return _menuItemsRepository.GetFilteredMenuItems(menuId, categoryId);
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            _menuItemsRepository.AddMenuItem(menuItem);
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            _menuItemsRepository.UpdateMenuItem(menuItem);
        }

        public void DeactivateMenuItem(int menuItemId)
        {
            _menuItemsRepository.DeactivateMenuItem(menuItemId);
        }

        public void ActivateMenuItem(int menuItemId)
        {
            _menuItemsRepository.ActivateMenuItem(menuItemId);
        }
    }
}
