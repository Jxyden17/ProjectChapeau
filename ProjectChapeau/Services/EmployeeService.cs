using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPasswordService _passwordService;

        public EmployeeService(IEmployeeRepository employeeRepository, IPasswordService passwordService)
        {
            _employeeRepository = employeeRepository;
            _passwordService = passwordService;
        }

        public void AddEmployee(Employee employee)
        {
            if (_employeeRepository.UserNameExists(employee.userName))
                throw new InvalidOperationException("A user with this username already exists");

            employee.salt = _passwordService.GenerateSalt();
            string interleavedSaltedPassword = _passwordService.InterleaveSalt(employee.password, employee.salt);
            string hashedPassword = _passwordService.HashPassword(interleavedSaltedPassword);

            Employee copyEmployee = employee;
            copyEmployee.password = _passwordService.HashPassword(interleavedSaltedPassword);

            _employeeRepository.Add(copyEmployee);

            if (employee.employeeId != copyEmployee.employeeId)
                employee.employeeId = copyEmployee.employeeId;
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAll();
        }

        public Employee? GetUserByLoginCredentials(string userName, string password)
        {

            return _employeeRepository.GetByLoginCredentials(userName, password);
        }
    }
}
