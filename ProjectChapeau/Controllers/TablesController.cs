using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Validation.Interfaces;


namespace ProjectChapeau.Controllers
{
    public class TablesController : Controller
    {
        private readonly ITableService _tableService;
        private readonly IOrderService _orderService;
        private readonly ITableEditValidator _tableEditValidator;

        public TablesController(ITableService tableService, IOrderService orderService, ITableEditValidator tableEditValidator)
        {
            _tableService = tableService;
            _orderService = orderService;
            _tableEditValidator = tableEditValidator;
        }


        //Initial page load with all tables and active order.
        public IActionResult Index()
        {
            List<Order> tableOrders = _tableService.GetAllTablesWithLatestOrder();

            return View(tableOrders);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        //Edit page proccessing with validator to keep controller lightweight.
        [HttpPost]
        public IActionResult Edit(TableEditViewModel tableEditViewModel)
        {
            try
            {
                //Retruieve current table and order from db
                TableEditViewModel tableEdit = _tableService.GetTableWithLatestOrderById(tableEditViewModel.tableID);

                //Send through validation.
                TableValidationResult validation = _tableEditValidator.ValidateTableEdit(tableEdit, tableEditViewModel);

                return UpdateTableAndRedirect(tableEditViewModel, validation);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex);
                tableEditViewModel.orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
                return View(tableEditViewModel);
            }
        }

        private IActionResult UpdateTableAndRedirect(TableEditViewModel tableEditViewModel, TableValidationResult validation)
        {
            if (!validation.IsValid)
            {
                ViewBag.ErrorMessage = validation.ErrorMessage;
                tableEditViewModel.orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
                return View(tableEditViewModel);
            }

            // If validation returns true this is ran and updates based on if the bool UpdateTable or UpdateOrder is True.
            if (validation.UpdateTable)
            {
                _tableService.UpdateTableStatus(tableEditViewModel.tableID, tableEditViewModel.isOccupied);
            }
            if (validation.UpdateOrder)
            {
                _orderService.UpdateOrderStatus(tableEditViewModel.orderId, tableEditViewModel.currentOrderStatus);
            }

            TempData["ConfirmMessage"] = "Table and/or order status updated successfully.";
            return RedirectToAction("Index");
        }


        //Inital edit page load that return the TableOrder through a ViewModel to the view.
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
