using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services;

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

            TableOrderModel tableOrderModel = new TableOrderModel
            {
                restaurantTables = restaurantTables,
                Orders = Orders
            };

            return View(tableOrderModel);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        //edit
        [HttpPost]
        public IActionResult Edit(RestaurantTable table)
        {
            try
            {
                _tableService.UpdateTable(table);
                TempData["ConfirmMessage"] = "Your employee has been edited succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                return View(table);
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
            return View(table);

        }

    }
}
