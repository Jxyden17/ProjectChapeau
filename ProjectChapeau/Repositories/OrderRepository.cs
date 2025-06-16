using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services;

namespace ProjectChapeau.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string? _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }
        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT 
                o.order_id,
                o.order_datetime,
                o.order_status,
                o.payment_status,
                e.employee_number, e.firstname, e.lastname, e.username, e.password, e.salt, e.is_active, e.role AS role_number,
                r.role_name,
                rt.table_number, rt.is_occupied
                FROM Orders o
                JOIN Employees e ON o.employee_number = e.employee_number
                JOIN Role r ON e.role = r.role_number
                JOIN RESTAURANT_TABLE rt ON o.table_number = rt.table_number;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = ReadEmployee(reader);
                    RestaurantTable restaurantTable = ReadTables(reader);

                    List<OrderItem> orderItems = new List<OrderItem>();


                    OrderStatus orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString());
                    paymentStatus paymentStatus = Enum.Parse<paymentStatus>(reader["payment_status"].ToString());

                    Order order = new Order(
                        (int)reader["order_id"],
                        employee,
                        restaurantTable,
                        orderItems,
                        (DateTime)reader["order_datetime"],
                        orderStatus,
                        paymentStatus,
                        1.0m,
                        1.0m,
                        1.0m
                        );

                    orders.Add(order);
                }
                reader.Close();
            }

            return orders;
        }

        public Order GetOrder(int id)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetRunningOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @" SELECT 
                o.order_id,
                o.order_datetime,
                o.order_status,
                o.payment_status,
                e.employee_number, e.firstname, e.lastname, e.username, e.password, e.salt, e.is_active, e.role AS role_number,
                r.role_name,
                rt.table_number, rt.is_occupied
                FROM Orders o
                JOIN Employees e ON o.employee_number = e.employee_number
                JOIN Role r ON e.role = r.role_number
                JOIN RESTAURANT_TABLE rt ON o.table_number = rt.table_number
    
                WHERE o.order_status = 'BeingPrepared'
                ORDER BY o.order_datetime ASC;";
                SqlCommand command = new SqlCommand(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = ReadEmployee(reader);
                    RestaurantTable restaurantTable = ReadTables(reader);
                    List<OrderItem> OrderItems = new List<OrderItem>();
                    OrderStatus orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString());
                    paymentStatus paymentStatus = Enum.Parse<paymentStatus>(reader["payment_status"].ToString());

                    Order order = new Order(
                        (int)reader["order_id"],
                        employee,
                        restaurantTable,
                        OrderItems,
                        (DateTime)reader["order_datetime"],
                        orderStatus,
                        paymentStatus,
                        1.0m,    
                        1.0m,
                        1.0m
                        );

                    orders.Add(order);
                }
                reader.Close();
            }

            return orders;
        }

        public void UpdateOrderStatus(int? orderId, OrderStatus? newStatus)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Orders SET order_status = @OrderStatus " +
                               "WHERE order_id = @OrderId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@OrderStatus", newStatus.ToString());

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
            Role employeeRole = new Role
            {
                roleId = (int)reader["role_number"],
                roleName = (string)reader["role_name"]
            };

            return new Employee(id, firstname, lastname, username, password, isActive, employeeRole, salt);
        }

        private RestaurantTable ReadTables(SqlDataReader reader)
        {
            int id = (int)reader["table_number"];
            bool IsOccupoed = (bool)reader["is_occupied"];

            return new RestaurantTable(id, IsOccupoed);
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            int orderId = (int)reader["order_id"];
            int menuItemId = (int)reader["menu_item_id"];
            string orderLineStatus = (string)reader["order_line_status"];
            string comment = (string)reader["comment"];
            int amount = (int)reader["amount"];

            return new OrderItem(menuItemId, orderId, amount, orderLineStatus, comment);
        }

        public List<Order> GetOrderByPeriod(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT o.order_id, o.order_datetime, o.order_status, o.payment_status, oi.amount, o.tip_amount,
                                 mi.menu_item_id, mi.price
                                 m.menu_name AS item_category, 
                                 (mi.price * oi.amount) AS sales_amount,
                                 ((mi.price * oi.amount) + o.tip_amount) AS income_amount
                                 FROM Orders o
                                 JOIN order_item oi ON o.order_id = oi.order_id
                                 JOIN Menu_Item mi ON oi.menu_item_id = mi.menu_item_id
                                 JOIN Menu_Contains_Item mci ON mi.menu_item_id = mci.menu_item_id
                                 JOIN Menu m ON mci.menu_id = m.menu_id
                                 WHERE o.order_datetime >= @StartDate AND o.order_datetime <= @EndDate 
                                 AND o.payment_status = 'Paid'";

                SqlCommand command = new SqlCommand(query, connection); 
                command.Parameters.AddWithValue("@StartDate",startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        orderId = (int)reader["order_id"],
                        datetime = (DateTime)reader["order_datetime"],
                        orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString()),
                        paymentStatus = Enum.Parse<paymentStatus>(reader["payment_status"].ToString()),
                        SalesAmount = (decimal)reader["sales_amount"],
                        IncomeAmount = (decimal)reader["income_amount"],
                        TipAmount = (decimal)reader["tip_amount"],
                        Category = reader["item_category"].ToString()

                    });
                }
            }
            return orders;
        }
    }
}