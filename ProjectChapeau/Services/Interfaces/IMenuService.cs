using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IMenuService
    {
        List<int> GetAllMenuIds();
        List<Menu> GetAllMenus();
        Menu GetMenuById(int menuId);
        // Returns a menu of menu items that has no assigned menu
        Menu GetMenuItemsWithoutDefinedMenu(string menuName);
    }
}
