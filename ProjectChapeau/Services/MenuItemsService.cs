using ProjectChapeau.Models;
using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class MenuItemsService : IMenuItemsService
    {
        private readonly IMenuItemsRepository _menuItemsRepository;

        public MenuItemsService(IMenuItemsRepository menuItemsRepository)
        {
            _menuItemsRepository = menuItemsRepository;
        }
        public List<Menu> GetAllMenuItems()
        {
            Menu lunchMenu = new(1, "Lunch Menu", GetMenuItemsByMenu(1));
            Menu dinnerMenu = new(2, "Dinner Menu", GetMenuItemsByMenu(2));
            Menu drinkMenu = new(3, "Drink Menu", GetMenuItemsByMenu(3));
            List<Menu> menus =
            [
                lunchMenu,
                dinnerMenu,
                drinkMenu
            ];
            return menus;
        }
        public List<MenuItem> GetMenuItemsByMenu(int menuId)
        {
            return _menuItemsRepository.GetMenuItemsByMenu(menuId);
        }
        public MenuItem? GetMenuItemById(int menuItemId)
        {
            return _menuItemsRepository.GetMenuItemById(menuItemId);
        }
    }
}
