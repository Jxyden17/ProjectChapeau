using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class TableRepository : BaseRepository, ITableRepository
    {
        public TableRepository(IConfiguration configuration) : base(configuration)
        {}

        public List<RestaurantTable> GetAllTables()
        {
            List<RestaurantTable> restaurantTables = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = "SELECT table_number, is_occupied FROM RESTAURANT_TABLE";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    RestaurantTable restaurantTable = ReadTable(reader);
                    restaurantTables.Add(restaurantTable);
                }
                reader.Close();
            }
            return restaurantTables;
        }
        public List<int> GetAllTableNumbers()
        {
            List<int> tableNumbers = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = "SELECT DISTINCT table_number FROM RESTAURANT_TABLE ORDER BY table_number";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tableNumbers.Add((int)reader["table_number"]);
                }
                reader.Close();
            }
            return tableNumbers;
        }

        public RestaurantTable GetTableByNumber(int tableNumber)
        {
            RestaurantTable table = new();
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT table_number, is_occupied
                                 FROM RESTAURANT_TABLE
                                 WHERE table_number = @TableNumber;";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@TableNumber", tableNumber);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        table = ReadTable(reader);
                    }
                }
            }
            return table;
        }

        public void UpdateTableStatus(int tableId, bool isOccupied)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "UPDATE RESTAURANT_TABLE SET is_occupied = @isOccupied " +
                               "WHERE table_number = @Id";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@Id", tableId);
                command.Parameters.AddWithValue("@IsOccupied", isOccupied);

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }
    }
}
