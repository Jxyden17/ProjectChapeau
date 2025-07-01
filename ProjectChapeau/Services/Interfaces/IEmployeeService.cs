using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee? GetEmployeeByNumber(int employeeNumber);
        Employee? GetEmployeeByLoginCredentials(string userName, string password);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);

        void DeactivateEmployee(int employeeId);
        void ActivateEmployee(int employeeId);
    }
}
