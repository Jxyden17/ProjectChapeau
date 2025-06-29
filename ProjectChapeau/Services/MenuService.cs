using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }
        public List<Menu> GetAllMenus()
        {
            List<Menu> menus = new();
            List<int> menuIds = GetAllMenuIds();
            // Get all defined menus
            foreach (int menuId in menuIds)
            {
                menus.Add(GetMenuById(menuId));
            }
            // Get all menu items without a defined menu
            menus.Add(GetMenuItemsWithoutDefinedMenu("Menu items without a defined menu"));
            return menus;
        }
        public List<int> GetAllMenuIds()
        {
            return _menuRepository.GetAllMenuIds();
        }
        public Menu GetMenuById(int menuId)
        {
            return _menuRepository.GetMenuById(menuId);
        }
        public Menu GetMenuItemsWithoutDefinedMenu(string menuName)
        {
            return _menuRepository.GetMenuItemsWithoutDefinedMenu(menuName);
        }
    }
}
