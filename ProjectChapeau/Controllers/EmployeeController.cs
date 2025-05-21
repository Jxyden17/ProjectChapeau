using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Views.Employee;

namespace ProjectChapeau.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {

            Employee? loggedInEmployee = HttpContext.Session.GetObject<Employee>("LoggedInEmployee");

            ViewData["LoggedInEmployee"] = loggedInEmployee;

            List<Employee> employees = _employeeService.GetAllEmployees();
            return View(employees);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login loginModel)
        {
            Employee? Employee = _employeeService.GetUserByLoginCredentials(loginModel.UserName, loginModel.Password);
            try
            {
                if (Employee == null)
                {
                    ViewBag.ErrorMessage = "Bad Username/Password Combo";
                    return View(loginModel);
                }
                else
                {

                    HttpContext.Session.SetObject("LoggedInEmployee", Employee);
                    return RedirectToAction("Index", "Employee");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View(loginModel);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedInEmployee");
            return RedirectToAction("Login", "Employee");
        }

        //Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                _employeeService.AddEmployee(employee);
                TempData["ConfirmMessage"] = "User added succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                return View(employee);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
    }
}
