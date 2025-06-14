using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Validation.Interfaces;
using System.ComponentModel.DataAnnotations;

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
                TableValidationResult validation = _tableEditValidator.ValidateTableEdit(tableEdit, tableEditViewModel);

                if (!validation.IsValid)
                {
                    ViewBag.ErrorMessage = validation.ErrorMessage;
                    tableEditViewModel.orderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
                    return View(tableEditViewModel);
                }
                _tableService.UpdateTableStatus(tableEditViewModel.tableID, tableEditViewModel.isOccupied);
                _orderService.UpdateOrderStatus(tableEditViewModel.orderId, tableEditViewModel.currentOrderStatus);

                TempData["ConfirmMessage"] = "Table and/or order status updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex);
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
