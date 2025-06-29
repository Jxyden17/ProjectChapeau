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

        public List<Order> GetAllTablesWithLatestOrder()
        {
            List<Order> Orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // This query gets each table and the latest order (if any) for that table
                // Query selecteer alles wat nodig voor een edit en status bepaling.
                string query = @"
                    SELECT 
                    t.table_number, 
                    t.is_occupied,

                    o.order_id,
                    o.order_status,
                    o.order_datetime,
                    o.payment_status,
                    o.tip_amount,
                    o.income_amount,

                    e.employee_number,
                    e.firstname,
                    e.lastname,
                    e.username,
                    e.password,
                    e.salt,
                    e.is_active,
                    e.role

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
                LEFT JOIN Employees e ON o.employee_number = e.employee_number
                ORDER BY t.table_number ASC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = ReadOrder(reader);
                    Orders.Add(order);
                }

                reader.Close();
            }

            return Orders;
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
                    o.order_status,
                    o.order_datetime,
                    o.payment_status,
                    o.tip_amount,
                    o.income_amount,

                    e.employee_number,
                    e.firstname,
                    e.lastname,
                    e.username,
                    e.password,
                    e.salt,
                    e.is_active,
                    e.role

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
                LEFT JOIN Employees e ON o.employee_number = e.employee_number
                WHERE t.table_number = @id
                ORDER BY t.table_number ASC;";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Order order = ReadOrder(reader);

                        IEnumerable<OrderStatus> statusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

                        return new TableEditViewModel(order.table.TableNumber, order.orderId, order.table.IsOccupied, order.orderStatus, statusOptions);
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

            return new Employee(id, firstname, lastname, username, password, isActive, employeeRole, salt);
        }

        private RestaurantTable ReadTables(SqlDataReader reader)
        {
            int id = (int)reader["table_number"];
            bool IsOccupoed = (bool)reader["is_occupied"];

            return new RestaurantTable(id, IsOccupoed);
        }

        private Order ReadOrder(SqlDataReader reader)
        {
            int orderId = (int)reader["order_id"];
            RestaurantTable table = ReadTables(reader);
            Employee employee = ReadEmployee(reader);
            List<OrderItem>? OrderItems = new List<OrderItem>();
            DateTime dateTime = (DateTime)reader["order_datetime"];
            OrderStatus orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString());
            paymentStatus paymentStatus = Enum.Parse<paymentStatus>(reader["payment_status"].ToString());
            decimal IncomeAmount = (decimal)reader["income_amount"];
            decimal tipAmount = (decimal)reader["tip_amount"];
            decimal SalesAmount = IncomeAmount + tipAmount;

            return new Order(orderId, employee, table, OrderItems, dateTime, orderStatus, paymentStatus, SalesAmount, IncomeAmount, tipAmount);

        }

    }
}
