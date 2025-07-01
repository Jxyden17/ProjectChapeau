using AspNetCoreGeneratedDocument;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemsRepository)
        {
            _menuItemRepository = menuItemsRepository;
        }
        
        public MenuItem GetMenuItemById(int menuItemId)
        {
            return _menuItemRepository.GetMenuItemById(menuItemId);
        }
        public List<MenuItem> GetMenuItemsByMenuId(int menuId)
        {
            return _menuItemRepository.GetMenuItemsByMenuId(menuId);
        }

        public List<MenuItem> GetMenuItemsWithoutMenuId()
        {
            return _menuItemRepository.GetMenuItemsWithoutMenuId();
        }

        public List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId)
        {
            return _menuItemRepository.GetFilteredMenuItems(menuId, categoryId);
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            _menuItemRepository.AddMenuItem(menuItem);
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            _menuItemRepository.UpdateMenuItem(menuItem);
        }

        public void DeactivateMenuItem(int menuItemId)
        {
            _menuItemRepository.DeactivateMenuItem(menuItemId);
        }

        public void ActivateMenuItem(int menuItemId)
        {
            _menuItemRepository.ActivateMenuItem(menuItemId);
        }
    }
}
