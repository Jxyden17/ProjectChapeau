using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuItemsRepository
    {
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(string menuName);
    }
}
