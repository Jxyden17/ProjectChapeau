using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IMenuItemsService
    {
        List<Menu> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(int menuId);
    }
}
