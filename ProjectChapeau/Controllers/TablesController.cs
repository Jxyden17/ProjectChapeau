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

            List<TableOrder> tableOrders =  new List<TableOrder>();

            foreach (RestaurantTable table in restaurantTables)
            {
                Order? latestOrder = Orders.Where(o => o.table.TableNumber == table.TableNumber).OrderByDescending(o => o.datetime).FirstOrDefault();

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
                else
                {
                    // Table is free and no active order
                    cardColor = "bg-success text-white";
                    statusText = "Available";
                }

                TableOrder tableOrder = new TableOrder(table.TableNumber, statusText, cardColor);
                tableOrders.Add(tableOrder);
            }

            return View(tableOrders);
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
