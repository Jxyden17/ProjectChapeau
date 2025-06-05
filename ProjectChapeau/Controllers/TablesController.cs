using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Controllers
{
    public class TablesController : Controller
    {
        private readonly ITableService _tableService;
        private readonly IOrderService _orderService;

        public TablesController(ITableService tableService, IOrderService orderService)
        {
            _tableService = tableService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            Employee? loggedInEmployee = HttpContext.Session.GetObject<Employee>("LoggedInEmployee");

            ViewData["LoggedInEmployee"] = loggedInEmployee;

            List<RestaurantTable> restaurantTables = _tableService.GetAllTables();
            List<Order> Orders = _orderService.GetAllOrders();

            List<TableViewModel> tableOrders = GetTableOrders(restaurantTables, Orders);

            return View(tableOrders);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        //edit
        [HttpPost]
        public IActionResult Edit(TableEditViewModel tableEditViewModel)
        {
            try
            {
                List<Order> Orders =  _orderService.GetAllOrders();
                Order? latestOrder = GetLatestOrder(Orders, tableEditViewModel.table);

                // Scenario 1: No active order for the table
                if (latestOrder == null || latestOrder.orderStatus == OrderStatus.Completed)
                {
                    // Update the table status (occupied/free)
                    _tableService.UpdateTableStatus(tableEditViewModel.table);

                    TempData["ConfirmMessage"] = "Your table has been edited successfully.";
                    return RedirectToAction("Index");
                }

                if (tableEditViewModel.order == null)
                {
                    return ReturnWithError("Table has an active order. Set the order status to Completed before editing the table.");

                }

                OrderStatus requestedStatus = tableEditViewModel.order.orderStatus;
                OrderStatus currentStatus = latestOrder.orderStatus;

                // Only allow ReadyToBeServed -> Served transition
                if (requestedStatus == OrderStatus.Served && currentStatus != OrderStatus.ReadyToBeServed)
                {
                    return ReturnWithError("You can only set the status to Served if the current order status is ReadyToBeServed.");
                }

                // Only allow changing from ReadyToBeServed to Served (block all other changes if running)
                if (currentStatus == OrderStatus.ReadyToBeServed && requestedStatus != OrderStatus.Served)
                {
                    return ReturnWithError("You can only change 'ReadyToBeServed' orders to 'Served'.");
                }

                // Only update if the status is actually changing
                if (requestedStatus == currentStatus)
                {
                    return ReturnWithError("No changes detected in order status.");
                }

                // All checks passed, update order
                latestOrder.orderStatus = requestedStatus;
                _orderService.UpdateOrderStatus(latestOrder);

                TempData["ConfirmMessage"] = "Order status updated successfully.";
                return RedirectToAction("Index");


            }
            catch (Exception ex)
            {
                
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";

                tableEditViewModel.orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

                return View(tableEditViewModel);
            }

            IActionResult ReturnWithError(string message)
            {
                ViewBag.ErrorMessage = message;
                tableEditViewModel.orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
                return View(tableEditViewModel);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RestaurantTable table =  _tableService.GetTableById((int)id);
            List<Order> orders = _orderService.GetAllOrders();
            Order? latestOrder = GetLatestOrder(orders, table);

            IEnumerable<OrderStatus> orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            TableEditViewModel tableEditViewModel = new TableEditViewModel(table, latestOrder, orderStatusOptions);

            return View(tableEditViewModel);

        }

        public List<TableViewModel> GetTableOrders(List<RestaurantTable> restaurantTables, List<Order> Orders)
        {
            List<TableViewModel> tableOrders = new List<TableViewModel>();

            foreach (RestaurantTable table in restaurantTables)
            {

                Order? latestOrder = GetLatestOrder(Orders, table);    

                string cardColor = "bg-success text-white";
                string statusText = "Available";

                if (latestOrder != null && latestOrder.orderStatus != OrderStatus.Completed)
                {
                    // Active order exists
                    cardColor = table.IsOccupied ? "bg-danger text-dark" : "bg-warning text-dark";
                    statusText = $"Order {latestOrder.orderStatus}";
                }
                else if (table.IsOccupied)
                {
                    // No active order, but table is still occupied
                    cardColor = "bg-warning text-dark";
                    statusText = "Occupied";
                }

                TableViewModel tableOrder = new TableViewModel(table.TableNumber, statusText, cardColor);
                tableOrders.Add(tableOrder);

                
            }
            return tableOrders;
        }

        public Order? GetLatestOrder(List<Order> orders, RestaurantTable table)
        {
            Order? latestOrder = orders.Where(o => o.table.TableNumber == table.TableNumber).OrderByDescending(o => o.datetime).FirstOrDefault();
            return latestOrder;
        }

        

    }
}
