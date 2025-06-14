using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Extensions;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;

namespace ProjectChapeau.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService) 
        {
            _employeeService = employeeService;
        }

        //index
        public IActionResult Index()
        {

            Employee? loggedInEmployee = HttpContext.Session.GetObject<Employee>("LoggedInEmployee");

            ViewData["LoggedInEmployee"] = loggedInEmployee;

            List<Employee> employees = _employeeService.GetAllEmployees();
            return View(employees);
        }


        //login&logout
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login loginModel)
        {
            Employee? Employee = _employeeService.GetEmployeeByLoginCredentials(loginModel.UserName, loginModel.Password);
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
        public ActionResult Create(EmployeeRoleModel employeeRoleModel)
        {
            try
            {
                
                _employeeService.AddEmployee(employeeRoleModel.employee);
                TempData["ConfirmMessage"] = "User added succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
               
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                employeeRoleModel.employee = new Employee();
                employeeRoleModel.Roles = _employeeService.GetAllEmployeeRoles();
                return View(employeeRoleModel);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            Employee employee = new Employee();
            List<Role> Roles = _employeeService.GetAllEmployeeRoles();
            EmployeeRoleModel viewModel = new EmployeeRoleModel(employee, Roles);

            return View(viewModel);
        }
        

        //Edit
        [HttpPost]
        public ActionResult Edit(EmployeeRoleModel employeeRoleModel)
        {
            try
            {
                _employeeService.UpdateEmployee(employeeRoleModel.employee);
                TempData["ConfirmMessage"] = "Your employee has been edited succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                employeeRoleModel.employee = new Employee();
                employeeRoleModel.Roles = _employeeService.GetAllEmployeeRoles();
                return View(employeeRoleModel);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee? employee = _employeeService.GetEmployeeById((int)id);
            List<Role> Roles = _employeeService.GetAllEmployeeRoles();
            EmployeeRoleModel viewModel = new EmployeeRoleModel(employee, Roles);
            return View(viewModel);

        }

        //Delete
        [HttpPost]
        public ActionResult Delete(Employee employee)
        {
            try
            {
                _employeeService.DeleteEmployee(employee);
                TempData["ConfirmMessage"] = "Your user has been deleted succesfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                return View(employee);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee? employee = _employeeService.GetEmployeeById((int)id);
            return View(employee);
        }

        public IActionResult Deactivate(int id)
        {
            _employeeService.DeactivateEmployee(id);
            return RedirectToAction("Index");
        }

        public IActionResult Activate(int id)
        {
            _employeeService.ActivateEmployee(id);
            return RedirectToAction("Index");
        }
    }
}
