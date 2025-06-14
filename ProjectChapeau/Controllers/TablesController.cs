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

            List<TableViewModel> tableOrders = _tableService.GetAllTablesWithLatestOrder();

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
                TableEditViewModel tableEdit = _tableService.GetTableWithLatestOrderById(tableEditViewModel.tableID);

                if (tableEdit == null || tableEditViewModel.orderId == null)
                {
                    ReturnWithError("Table or Order not found. try again.");
                }
                // Scenario 1: No active order for the table
                Console.WriteLine($"{tableEdit.currentOrderStatus} +++++++++++ {tableEdit.currentOrderStatus}");
                if (tableEdit.currentOrderStatus == null || tableEdit.currentOrderStatus == OrderStatus.Completed)
                {
                    // Update the table status (occupied/free)
                    _tableService.UpdateTableStatus(tableEditViewModel.tableID, tableEditViewModel.isOccupied);

                    TempData["ConfirmMessage"] = "Your table has been edited successfully.";
                    return RedirectToAction("Index");
                }

                // If there is an active order, only allow valid status transitions
                OrderStatus? requestedStatus = tableEditViewModel.currentOrderStatus;
                OrderStatus currentStatus = tableEdit.currentOrderStatus.Value;

                bool isTableStatusChanging = tableEdit.isOccupied != tableEditViewModel.isOccupied;

                bool isOrderStatusChanging = currentStatus != requestedStatus;

                Console.WriteLine($"{isOrderStatusChanging}, ++++++++++++++ {isTableStatusChanging} +++++++++++++++++++ {requestedStatus} +++++++++++++++++++++++++ {currentStatus}");

                // Only allow ReadyToBeServed -> Served transition
                if (isOrderStatusChanging)
                {
                    if (requestedStatus == OrderStatus.Served && currentStatus != OrderStatus.ReadyToBeServed)
                    {
                        return ReturnWithError("You can only set the status to Served if the current order status is ReadyToBeServed.");
                    }
                    if (currentStatus == OrderStatus.ReadyToBeServed && requestedStatus != OrderStatus.Served)
                    {
                        return ReturnWithError("You can only change 'ReadyToBeServed' orders to 'Served'.");
                    }
                    // All checks passed, update order status
                    Console.WriteLine($"Check of dit gerunned is.");
                    _orderService.UpdateOrderStatus(tableEditViewModel.orderId, requestedStatus);
                }

                if (isTableStatusChanging)
                {
                    _tableService.UpdateTableStatus(tableEditViewModel.tableID, tableEditViewModel.isOccupied);
                }

                if (isOrderStatusChanging || isTableStatusChanging)
                {
                    TempData["ConfirmMessage"] = "Table and/or order status updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    return ReturnWithError("No changes detected in table or order status.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex);
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

            TableEditViewModel tableEditViewModel = _tableService.GetTableWithLatestOrderById(id);

            return View(tableEditViewModel);

        }


        

    }
}
