using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;

using ProjectChapeau.Views.ViewModel;


namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {

        private readonly IMenuItemService _menuItemsService;
        
        private readonly IOrderService _orderService;
       
            public IActionResult Index()
        {
            List<Order> runningOrders = _orderService.GetRunningOrders();
            return View(runningOrders);

        }
        public OrderController(IMenuItemService menuItemsService)
        {
            _menuItemsService = menuItemsService;
        }
         public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
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
            return View(menuItem);

   
    }
}