using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuItemsRepository
    {
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(int menuId);
        MenuItem? GetMenuItemById(int menuItemId);
    }
}
