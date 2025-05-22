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

        public RestaurantTable GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT table_number, is_occupied FROM RESTAURANT_TABLE
                        WHERE table_number = @TableNumber;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableNumber", id);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        RestaurantTable table = ReadTables(reader);
                        return table;
                    }
                }
            }
            return null;
        }

        public void UpdateTable(RestaurantTable table)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RESTAURANT_TABLE SET is_occupied = @isOccupied " +
                               "WHERE table_number = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", table.TableNumber);
                command.Parameters.AddWithValue("@IsOccupied", table.IsOccupied);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        private RestaurantTable ReadTables(SqlDataReader reader)
        {
            int id = (int)reader["table_number"];
            bool IsOccupoed = (bool)reader["is_occupied"];

            return new RestaurantTable(id, IsOccupoed);
        }
    }
}
