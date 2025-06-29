using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Data;


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

            List<Employee> employees = _employeeService.GetAllEmployees();
            return View(employees);
        }


        //login&logout

        //Load login form on page load.
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(Login loginModel)
        {
            try
            {
                // Retrieve employee with username and password
                Employee? employee = _employeeService.GetEmployeeByLoginCredentials(loginModel.UserName, loginModel.Password);

                return LoginUserAndRedirect(loginModel, employee);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occured: {ex.Message}";
                return View(loginModel);
            }
        }

        private IActionResult LoginUserAndRedirect(Login loginModel, Employee? employee)
        {
            if (employee == null)
            {
                ViewBag.ErrorMessage = "Bad Username/Password Combo";
                return View(loginModel);
            }

            //Create user claims
            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employee.userName), // or Username if preferred
                    new Claim("EmployeeId", employee.employeeId.ToString()),
                    new Claim(ClaimTypes.Role, employee.role.ToString()) // E.g., "Owner", "Waiter"
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };

            //Sign in the user using cookie authentication
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            // Redirect based on roleId (same as before)
            if (employee.role == Roles.Administrator || employee.role == Roles.Owner || employee.role == Roles.Manager)
            {
                return RedirectToAction("Index", "Employee");
            }
            if(employee.role == Roles.KitchenStaff || employee.role == Roles.BarStaff)
            {
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return RedirectToAction("Index", "Tables");
            }
        }

        //When user presses logout button this logs the user out.
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
                employeeRoleModel.Roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
                return View(employeeRoleModel);
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            Employee employee = new Employee();
            List<Roles> Roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
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
                employeeRoleModel.Roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
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
            List<Roles> Roles = Enum.GetValues(typeof(Roles)).Cast<Roles>().ToList();
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
