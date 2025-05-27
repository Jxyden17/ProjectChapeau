using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMenuItemsService _menuItemsService;

        public OrdersController(IMenuItemsService menuItemsService)
        {
            _menuItemsService = menuItemsService;
        }
        public IActionResult Index()
        {
            // Placeholder redirect
            return RedirectToAction("Menu", "Orders");
        }
        public IActionResult Menu()
        {
            List<Menu> menus = _menuItemsService.GetAllMenuItems();
            return View(menus);
        }
        public IActionResult MenuItem(int? id)
        {
            if (id == null)
            {
                return NotFound("There is no menu item ID provided in the URL. Please go back and try again.");
            }
            MenuItem? menuItem = _menuItemsService.GetMenuItemById((int)id);

            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} does not exist");
            }
            return View(menuItem);
        }
    }
}
