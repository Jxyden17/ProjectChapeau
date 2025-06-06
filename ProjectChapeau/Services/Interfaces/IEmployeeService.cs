﻿using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee? GetById(int id);
        Employee? GetEmployeeByLoginCredentials(string userName, string password);
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee employee);

        void DeleteEmployee(Employee employee);
    }
}
