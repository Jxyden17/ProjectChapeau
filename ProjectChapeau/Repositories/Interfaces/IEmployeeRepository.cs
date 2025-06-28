using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        List<Employee> GetAllEmployees();
        Employee? GetEmployeeById(int UserId);
        Employee? GetEmployeeByUsername(string username);
        bool UserNameExists(string userName);
        void DeactivateEmployee(int employeeId);
        void ActivateEmployee(int employeeId);
        List<Role> GetAllEmployeeRoles();
    }
}
