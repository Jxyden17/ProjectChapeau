using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        List<Employee> GetAll();
        Employee? GetById(int UserId);
        Employee? GetByLoginCredentials(string username, string password);
        bool UserNameExists(string userName);
        void Deactivate(int employeeId);
        void Activate(int employeeId);
    }
}
