using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly string? _connectionString;

        public TableRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }
        public List<RestaurantTable> GetAllTables()
        {
            List<RestaurantTable> restaurantTables = new List<RestaurantTable>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT table_number, is_occupied FROM RESTAURANT_TABLE";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RestaurantTable restaurantTable = ReadTables(reader);
                    restaurantTables.Add(restaurantTable);
                }
                reader.Close();
            }
            return restaurantTables;
        }

        public RestaurantTable ReadTables(SqlDataReader reader)
        {
            int id = (int)reader["table_number"];
            bool IsOccupoed = (bool)reader["is_occupied"];

            return new RestaurantTable(id, IsOccupoed);
        }
    }
}
