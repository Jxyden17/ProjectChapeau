using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Views.ViewModel;

namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMenuItemService _menuItemsService;
        public IActionResult Index()
        {
            // Placeholder redirect
            return RedirectToAction("Menu", "Orders");
        }
        public OrderController(IMenuItemService menuItemsService)
        {
            _menuItemsService = menuItemsService;
        }
        public IActionResult Menu()
        {
            MenusOverviewViewModel menusOverviewViewModel = new(_menuItemsService.GetAllMenuItems());
            return View(menusOverviewViewModel);
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

            MenuItemOverviewViewModel menuItemOverviewViewModel = new(menuItem);

            return View(menuItemOverviewViewModel);
        }
    }
}
