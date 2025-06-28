using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        List<int> GetAllMenuIds();
        Menu GetMenuById(int menuId);
        // Returns a menu of menu items that has a NULL value in the menu_id column
        Menu GetMenuItemsWithoutDefinedMenu(string menuName);
    }
}
