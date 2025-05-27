using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IMenuItemsService
    {
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(string menuName);
    }
}
