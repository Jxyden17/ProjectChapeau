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
        public List<Menu> GetAllMenuItems()
        {
            Menu lunchMenu = new(1, "Lunch Menu", GetMenuItemsByMenu(1));
            Menu dinnerMenu = new(2, "Dinner Menu", GetMenuItemsByMenu(2));
            Menu drinkMenu = new(3, "Drink Menu", GetMenuItemsByMenu(3));
            Menu undefinedMenu = new(0, "Menu Items without menu", GetMenuItemsWithoutDefinedMenu());
            List<Menu> menus =
            [
                lunchMenu,
                dinnerMenu,
                drinkMenu,
                undefinedMenu
            ];
            return menus;
        }
        public List<MenuItem> GetMenuItemsByMenu(int menuId)
        {
            return _menuItemsRepository.GetMenuItemsByMenu(menuId);
        }
        public List<MenuItem> GetMenuItemsWithoutDefinedMenu()
        {
            return _menuItemsRepository.GetMenuItemsWithoutDefinedMenu();
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
