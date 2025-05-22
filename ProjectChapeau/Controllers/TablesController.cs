using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Views.ViewModel;

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
    }
}
