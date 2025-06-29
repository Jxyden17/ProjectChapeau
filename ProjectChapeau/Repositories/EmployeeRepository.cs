using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Repositories;
using ProjectChapeau.Services.Interfaces;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string? _connectionString;
        private readonly IPasswordService _passwordService;

        public EmployeeRepository(IConfiguration configuration, IPasswordService passwordService)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
            _passwordService = passwordService;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
                command.Parameters.AddWithValue("@Role", employee.role);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }
        public void DeleteEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT employee_number,  firstname,  lastname, username, password, salt, is_active, role
                        FROM Employees";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = ReadEmployee(reader);
                    employees.Add(employee);
                }
                reader.Close();
            }

            return employees;
        }

        public Employee? GetEmployeeById(int UserId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT employee_number,  firstname,  lastname, username, password, salt, is_active, role
                        FROM Employees 
                        WHERE employee_number = @UserId;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", UserId);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Employee employee = ReadEmployee(reader);
                        return employee;
                    }
                }
            }
            return null;
        }

        public Employee? GetEmployeeByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT employee_number, firstname, lastname, username, password, salt, is_active, role
                         FROM Employees 
                         WHERE username = @Username;";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Username", username);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return ReadEmployee(reader);
                }
                return null;
            }
        }

        private Employee ReadEmployee(SqlDataReader reader)
        {
            int id = (int)reader["employee_number"];
            string firstname = (string)reader["firstname"];
            string lastname = (string)reader["lastname"];
            string username = (string)reader["username"];
            string password = (string)reader["password"];
            string salt = (string)reader["salt"];
            bool isActive = (bool)reader["is_active"];
            Roles employeeRole = Enum.Parse<Roles>(reader["role"].ToString());

            return new Employee(id, firstname ,lastname,username , password , isActive, employeeRole, salt);
        }

        public bool UserNameExists(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
            using (SqlConnection connection = new(_connectionString))
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
            using (SqlConnection connection = new(_connectionString))
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
