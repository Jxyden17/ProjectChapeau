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

        public IActionResult CurrentOrder()
        {
            return View(GetOrCreateCurrentOrder());
        }

        [HttpPost]
        public IActionResult AddItemToOrder(int? menuItemId, int? amount, string? comment, string? returnTo)
        {
            if (!menuItemId.HasValue)
            {
                TempData["ErrorMessage"] = "No menu item specified.";
                return View();
            }
            try
            {
                Order order = GetOrCreateCurrentOrder();
                MenuItem menuItem = _menuItemService.GetMenuItemById(menuItemId.Value);
                OrderLine orderLine = CreateOrderLine(menuItem, amount ?? 1, comment);

                order.OrderLines.Add(orderLine);
                SaveCurrentOrderToSession(order);

                switch(returnTo)
                {
                    case "menu":
                        return RedirectToAction("Menu");
                    case "currentOrder":
                        return RedirectToAction("CurrentOrder");
                    default:
                        return RedirectToAction("MenuItem");

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Item could not be added to order: {ex.Message}";
                return View();
            }
        }

        // Private methods
        private Order GetOrCreateCurrentOrder()
        {
            var order = HttpContext.Session.GetObject<Order>(orderSessionKey);
            if (order == null)
            {
                order = CreateNewOrder();
                SaveCurrentOrderToSession(order);
            }
            return order;
        }

        private void SaveCurrentOrderToSession(Order order)
        {
            HttpContext.Session.SetObject(orderSessionKey, order);
        }

        private Order CreateNewOrder()
        {
            // TODO
            var employee = new Employee();
            var table = new RestaurantTable();
            return new Order(0, employee, table, new List<OrderLine>(), DateTime.Now, OrderStatus.NotOrdered, false, 0);
        }

        private OrderLine CreateOrderLine(MenuItem menuItem, int amount, string? comment)
        {
            return new OrderLine(menuItem, amount, comment, OrderStatus.NotOrdered);
        }

    }
}