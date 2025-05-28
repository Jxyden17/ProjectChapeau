using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IMenuItemService
    {
        List<Menu> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByMenu(int menuId);
        List<MenuItem> GetMenuItemsWithoutDefinedMenu();
        MenuItem? GetMenuItemById(int menuItemId);
    }
}