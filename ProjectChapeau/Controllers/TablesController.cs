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
            List<TableWithOrder> tablesWithOrder = _tableService.GetAllTablesWithLatestOrder();

            return View(tablesWithOrder);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //Inital edit page load that return the TableOrder through a ViewModel to the view.
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TableEditViewModel tableEditViewModel = _tableService.GetTableWithLatestOrder(id.Value);

            return View(tableEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(TableEditViewModel editedVm)
        {
            editedVm.OrderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            try
            {
                var currentVm = _tableService.GetTableWithLatestOrder(editedVm.Table.TableNumber)
                               ?? throw new InvalidOperationException("Table not found.");

                if (editedVm.Order == null)
                    editedVm.Order = new Order();

                TableValidationResult validation = _tableEditValidator.ValidateTableEdit(currentVm, editedVm);

                if (!validation.IsValid)
                {
                    ViewBag.ErrorMessage = validation.ErrorMessage;
                    return View(editedVm);
                }

                if (validation.UpdateTable)
                    _tableService.UpdateTableStatus(editedVm.Table.TableNumber, editedVm.Table.IsOccupied);

                if (validation.UpdateOrder)
                    _orderService.UpdateOrderStatus(editedVm.Order.OrderId, editedVm.Order.OrderStatus);

                TempData["ConfirmMessage"] = "Table and/or order status updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex);
                return View(editedVm);
            }
        }

        private IActionResult UpdateTableAndRedirect(TableEditViewModel tableEditViewModel, TableValidationResult validation)
        {
            if (!validation.IsValid)
            {
                ViewBag.ErrorMessage = validation.ErrorMessage;
                tableEditViewModel.OrderStatusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
                return View(tableEditViewModel);
            }

            // If validation returns true this is ran and updates based on if the bool UpdateTable or UpdateOrder is True.
            if (validation.UpdateTable)
            {
                _tableService.UpdateTableStatus(tableEditViewModel.Table.TableNumber, tableEditViewModel.Table.IsOccupied);
            }
            if (validation.UpdateOrder)
            {
                _orderService.UpdateOrderStatus(tableEditViewModel.Order.OrderId, tableEditViewModel.Order.OrderStatus);
            }

            TempData["ConfirmMessage"] = "Table and/or order status updated successfully.";
            return RedirectToAction("Index");
        }
    }
}
