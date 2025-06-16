using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
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

        public List<TableOrder> GetAllTablesWithLatestOrder()
        {
            List<TableOrder> AllTableOrders = new List<TableOrder>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // This query gets each table and the latest order (if any) for that table
                // Query selecteer alles wat nodig voor een edit en status bepaling.
                string query = @"
                SELECT 
                    t.table_number, 
                    t.is_occupied,
                    o.order_id,
                    o.order_status
                FROM RESTAURANT_TABLE t
                LEFT JOIN (
                    SELECT o1.*
                    FROM Orders o1
                    INNER JOIN (
                        SELECT table_number, MAX(order_datetime) AS MaxDate
                        FROM Orders
                        GROUP BY table_number
                    ) o2 ON o1.table_number = o2.table_number AND o1.order_datetime = o2.MaxDate
                ) o ON t.table_number = o.table_number
                ORDER BY t.table_number ASC;
            ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TableOrder tableOrder = ReadTableOrder(reader);
                    AllTableOrders.Add(tableOrder);
                }

                reader.Close();
            }

            return AllTableOrders;
        }

        public RestaurantTable GetTableById(int id)
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

        public TableEditViewModel GetTableWithLatestOrderById(int? id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT 
                    t.table_number, 
                    t.is_occupied,
                    o.order_id,
                    o.order_status         
                FROM RESTAURANT_TABLE t
                LEFT JOIN (
                    SELECT o1.*
                    FROM Orders o1
                    INNER JOIN (
                        SELECT table_number, MAX(order_datetime) AS MaxDate
                        FROM Orders
                        GROUP BY table_number
                    ) o2 ON o1.table_number = o2.table_number AND o1.order_datetime = o2.MaxDate
                ) o ON t.table_number = o.table_number
                WHERE t.table_number = @id;
                ";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TableOrder tableOrder = ReadTableOrder(reader);

                        IEnumerable<OrderStatus> statusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

                        return new TableEditViewModel(tableOrder.TableNumber, tableOrder.OrderId, tableOrder.IsOccupied, tableOrder.OrderStatus, statusOptions);
                    }
                }
            }
            return null;
        }

        public void UpdateTableStatus(int tableId, bool isOccupied)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE RESTAURANT_TABLE SET is_occupied = @isOccupied " +
                               "WHERE table_number = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", tableId);
                command.Parameters.AddWithValue("@IsOccupied", isOccupied);

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

        private TableOrder ReadTableOrder(SqlDataReader reader)
        {
            int tableNumber = (int)reader["table_number"];
            bool IsOccupied = (bool)reader["is_occupied"];
            int? OrderId = reader["order_id"] != DBNull.Value ? (int?)reader["order_id"] : null;
            OrderStatus? orderStatus = reader["order_status"] != DBNull.Value
                        ? Enum.Parse<OrderStatus>(reader["order_status"].ToString())
                        : (OrderStatus?)null;

            return new TableOrder(tableNumber, IsOccupied, OrderId, orderStatus);
        }
    }
}
