using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
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
            if (menuItemId == null)
            {
                TempData["ErrorMessage"] = "No menu item ID is specified to add to the order.";
                return RedirectToAction("MenuItem", new {id = menuItemId});
            }

            try
            {
                Order order = GetOrCreateCurrentOrder();
                MenuItem? menuItem = _menuItemService.GetMenuItemById(menuItemId.Value);
                int quantity = amount ?? 1;

                AddOrUpdateOrderLine(order, menuItem, quantity, comment);
                SaveCurrentOrderToSession(order);

                switch (returnTo)
                {
                    case "menu":
                        return RedirectToAction("Menu");
                    case "currentOrder":
                        return RedirectToAction("CurrentOrder");
                    default:
                        return RedirectToAction("MenuItem", new {id = menuItemId});

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Item could not be added to order: {ex.Message}";
                return RedirectToAction("MenuItem", new { id = menuItemId });
            }
        }

        [HttpGet]
        public IActionResult ChangeTable()
        {
            return View(GetOrCreateCurrentOrder());
        }
        [HttpPost]
        public IActionResult ChangeTable(int number)
        {
            try
            {
                Order order = GetOrCreateCurrentOrder();
                order.Table.TableNumber = number;
                SaveCurrentOrderToSession(order);
                return RedirectToAction("CurrentOrder");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"The table of the current order could not be changed: {ex.Message}";
                return View(number);
            }
        }

        [HttpPost]
        public IActionResult SendCurrentOrder()
        {
            Order order = GetOrCreateCurrentOrder();
            return RedirectToAction("Menu");
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

        private static Order CreateNewOrder()
        {
            // TODO
            var employee = new Employee();
            var table = new RestaurantTable();
            return new Order(0, employee, table, new(), DateTime.Now, OrderStatus.NotOrdered, false, 0);
        }

        private static OrderLine CreateOrderLine(MenuItem menuItem, int amount, string? comment)
        {
            return new OrderLine(menuItem, amount, comment, OrderStatus.NotOrdered);
        }

        private void AddOrUpdateOrderLine(Order order, MenuItem menuItem, int amount, string? comment)
        {
            OrderLine? existingOrderLine = FindExistingOrderLine(order, menuItem.MenuItemId);

            if (existingOrderLine != null)
            {
                UpdateOrderLine(existingOrderLine, amount, comment);
            }
            else
            {
                OrderLine newOrderLine = CreateOrderLine(menuItem, amount, comment);
                order.OrderLines.Add(newOrderLine);
            }
        }

        private OrderLine? FindExistingOrderLine(Order order, int menuItemId)
        {
            return order.OrderLines.FirstOrDefault(orderLine => orderLine.MenuItem.MenuItemId == menuItemId);
        }

        private void UpdateOrderLine(OrderLine orderLine, int amount, string? comment)
        {
            orderLine.Amount += amount;

            if (!string.IsNullOrWhiteSpace(comment))
            {
                orderLine.Comment = comment;
            }
        }
    }
}