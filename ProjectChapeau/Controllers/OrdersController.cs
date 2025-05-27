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
            Menu lunchMenu = new(1, "Lunch Menu", _menuItemsService.GetMenuItemsByMenu("Lunch Menu"));
            Menu dinnerMenu = new(2, "Dinner Menu", _menuItemsService.GetMenuItemsByMenu("Dinner Menu"));
            Menu drinkMenu = new(3, "Drink Menu", _menuItemsService.GetMenuItemsByMenu("Drink Menu"));
            List<Menu> menus =
            [
                lunchMenu,
                dinnerMenu,
                drinkMenu
            ];
            return View(menus);
        }
    }
}
