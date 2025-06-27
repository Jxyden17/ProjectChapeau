using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {

        private readonly IMenuItemService _menuItemsService;

        private readonly IOrderService _orderService;
        
        public OrderController(IMenuItemService menuItemsService, IOrderService orderService)
        {
            _menuItemsService = menuItemsService;
            _orderService = orderService;
        }
       
            public IActionResult Index()
        {
            List<Order> runningOrders = _orderService.GetRunningOrders();
            return View(runningOrders);

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