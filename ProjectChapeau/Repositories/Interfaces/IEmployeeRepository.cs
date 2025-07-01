using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Employee? GetEmployeeByNumber(int employeeNumber);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        List<Employee> GetEmployees(int? employeeNumber = null, string? username = null);
        bool UserNameExists(string userName);
        void DeactivateEmployee(int employeeId);
        void ActivateEmployee(int employeeId);
    }
}
