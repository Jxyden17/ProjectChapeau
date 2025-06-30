using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {
        private const string orderSessionKey = "OrderSession";

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
        public IActionResult StartOrder()
        {
            Order order = new();
            HttpContext.Session.SetObject(orderSessionKey, order);

            return View();
        }
        public IActionResult AddItemToOrder(int? menuItemId, int? amount, string? comment)
        {
            try
            {
                Order? order = HttpContext.Session.GetObject<Order>(orderSessionKey);

                if (menuItemId == null)
                {
                    throw new ArgumentNullException(nameof(menuItemId));
                }
                int quantity = amount ?? 1;
                MenuItem menuItem = _menuItemService.GetMenuItemById(menuItemId.Value);
                OrderLine orderLine = new(menuItem, quantity, comment, OrderStatus.NotOrdered);
                order.OrderLines.Add(orderLine);
                HttpContext.Session.SetObject<Order>(orderSessionKey, order);
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Item could not be added to current order: {ex.Message}";
                return View();
            }
        }
    }
}