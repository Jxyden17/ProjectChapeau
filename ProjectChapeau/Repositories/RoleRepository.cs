using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string? _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }
        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT role_number, role_name
                    FROM Role";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Role role = ReadRole(reader);
                    roles.Add(role);
                }
                reader.Close();
            }

            return roles;
        }

        public Role ReadRole(SqlDataReader reader)
        {
            int id = (int)reader["role_number"];
            string roleName = (string)reader["role_name"];


            return new Role(id,roleName);
        }
    }
}
