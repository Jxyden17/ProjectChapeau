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
        public List<MenuItem> GetAllMenuItems()
        {
            return _menuItemsRepository.GetAllMenuItems();
        }
        public List<MenuItem> GetMenuItemsByMenu(string menuName)
        {
            return _menuItemsRepository.GetMenuItemsByMenu(menuName);
        }
    }
}
