using ProjectChapeau.Models;
using System.Collections.Generic;

namespace ProjectChapeau.Models.ViewModel
{
    public class EmployeeRoleModel
    {
        public Models.Employee employee { get; set; }
        public List<Role> Roles { get; set; }

        public EmployeeRoleModel(Models.Employee employee, List<Role> roles)
        {
            this.employee = employee;
            Roles = roles;
        }

        public EmployeeRoleModel()
        {
        }



        //EmployeeRoleModel mag nooit null zijn anders errors in de view.


        // Parameterless constructor with default initialization

    }
}