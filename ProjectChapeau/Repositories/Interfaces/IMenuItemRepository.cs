using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAllMenuItems();
        MenuItem? GetMenuItemById(int menuItemId);
        List<MenuItem> GetCategory(int categoryId);

        List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId);
    }
}
