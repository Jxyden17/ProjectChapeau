using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee? GetUserByLoginCredentials(string userName, string password);
        void AddEmployee(Employee employee);
    }
}
