namespace ProjectChapeau.Models
{
    public class Employee
    {

        public int employeeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }
        public string roleName { get; set; }
        public string salt { get; set; }

        public Employee(int employeeId, string firstName, string lastName, string userName, string password, bool isActive, string RoleName, string salt)
        {
            this.employeeId = employeeId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.password = password;
            this.isActive = isActive;
            this.roleName = RoleName;
            this.salt = salt;
        }

        public Employee() { }
    }
}
