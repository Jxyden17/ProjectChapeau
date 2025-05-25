using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public IActionResult Index(int? menuId, int? categoryId)
        {
            var menuItems = _menuItemRepository.GetFilteredMenuItems(menuId, categoryId);
            return View(menuItems);


        }


    }

}
