using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IMenuItemService
    {
        MenuItem GetMenuItemById(int menuItemId);
        List<MenuItem> GetMenuItemsByMenuId(int menuId);
        List<MenuItem> GetMenuItemsWithoutMenuId();
        List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId);
        void AddMenuItem(MenuItem menuItem);
        void UpdateMenuItem(MenuItem menuItem);
        void DeactivateMenuItem(int menuItemId);
        void ActivateMenuItem(int menuItemId);
    }
}
