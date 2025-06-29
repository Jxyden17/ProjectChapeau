using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {

        private readonly IMenuService _menuService;
        private readonly IMenuItemService _menuItemService;
        private readonly IOrderService _orderService;
        
        public OrderController(IMenuService menuService, IMenuItemService menuItemService, IOrderService orderService)
        {
            _menuService = menuService;
            _menuItemService = menuItemService;
            _orderService = orderService;
        }
       
            public IActionResult Index()
        {
            List<Order> runningOrders = _orderService.GetRunningOrders();
            return View(runningOrders);

        }
       
        public IActionResult Menu()
        {
            Employee? loggedInEmployee = HttpContext.Session.GetObject<Employee>("LoggedInEmployee");
            ViewData["LoggedInEmployee"] = loggedInEmployee;

            MenusOverviewViewModel menusOverviewViewModel = new(_menuService.GetAllMenus());
            return View(menusOverviewViewModel);
        }
        public IActionResult MenuItem(int? id)
        {
            if (id == null)
            {
                return NotFound("There is no menu item ID provided in the URL. Please go back and try again.");
            }
            MenuItem? menuItem = _menuItemService.GetMenuItemById((int)id);

            if (menuItem == null)
            {
                return NotFound($"Menu item with ID {id} does not exist");
            }
            MenuItemOverviewViewModel menuItemOverviewViewModel = new(menuItem);

            return View(menuItemOverviewViewModel);
        }
    }
}