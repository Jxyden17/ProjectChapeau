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
    }
}
