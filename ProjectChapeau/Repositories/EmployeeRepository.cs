using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Repositories;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Repositories
{
    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        private readonly IPasswordService _passwordService;

        public EmployeeRepository(IConfiguration configuration, IPasswordService passwordService) : base(configuration) 
        {
            _passwordService = passwordService;
        }

        public Employee? GetEmployeeByNumber(int employeeNumber)
        {
            Employee? employee = null;

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT *
                                 FROM Employees
                                 WHERE employee_number = @EmployeeNumber;";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    employee = ReadEmployee(reader);
                }
                reader.Close();
            }
            return employee;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = $"INSERT INTO Employees (firstname, lastname, username, password, salt, is_active, role) " +
                               "VALUES (@FirstName, @LastName, @Username, @Password, @Salt, @IsActive, @Role); " +
                               "SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@FirstName", employee.firstName);
                command.Parameters.AddWithValue("@LastName", employee.lastName);
                command.Parameters.AddWithValue("@Username", employee.userName);
                command.Parameters.AddWithValue("@Password", employee.password);
                command.Parameters.AddWithValue("@Salt", employee.salt);
                command.Parameters.AddWithValue("@IsActive", employee.isActive);
                command.Parameters.AddWithValue("@Role", employee.role);

                command.Connection.Open();
                employee.employeeId = Convert.ToInt32(command.ExecuteScalar());
            }
        }
        public void UpdateEmployee(Employee employee)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "UPDATE Employees SET firstname = @FirstName, lastname = @LastName, username = @Username, password = @Password, salt = @Salt, is_active = @IsActive , role = @Role " +
                                "WHERE employee_number = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", employee.employeeId);
                command.Parameters.AddWithValue("@FirstName", employee.firstName);
                command.Parameters.AddWithValue("@LastName", employee.lastName);
                command.Parameters.AddWithValue("@Username", employee.userName);
                command.Parameters.AddWithValue("@Password", employee.password);
                command.Parameters.AddWithValue("@Salt", employee.salt);
                command.Parameters.AddWithValue("@IsActive", employee.isActive);
                command.Parameters.AddWithValue("@Role", employee.role.ToString());

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
            
        }
        public void DeleteEmployee(Employee employee)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = $"DELETE FROM Employees WHERE employee_number = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", employee.employeeId);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records deleted!");
            }
        }

        public List<Employee> GetEmployees(int? employeeNumber = null, string? username = null)
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT employee_number, firstname, lastname, username, password, salt, is_active, role
                         FROM Employees";
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();

                if (employeeNumber.HasValue)
                {
                    conditions.Add("employee_number = @EmployeeNumber");
                    command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber.Value);
                }
                if (!string.IsNullOrEmpty(username))
                {
                    conditions.Add("username = @Username");
                    command.Parameters.AddWithValue("@Username", username);
                }
                if (conditions.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                command.CommandText = query;
                command.Connection = connection;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(ReadEmployee(reader));
                    }
                }
            }

            return employees;
        }

        public bool UserNameExists(string userName)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "SELECT COUNT(*) FROM Employees WHERE username = @UserName";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserName", userName);

                connection.Open();

                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        public void DeactivateEmployee(int employeeId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "UPDATE Employees SET is_active = 0 WHERE employee_number = @employee_number";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@employee_number", employeeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ActivateEmployee(int employeeId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "UPDATE Employees SET is_active = 1 WHERE employee_number = @employee_number";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@employee_number", employeeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}