using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuItemsRepository
    {
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(int menuId);
        List<MenuItem> GetMenuItemsWithoutDefinedMenu();
        MenuItem? GetMenuItemById(int menuItemId);
    }
}
