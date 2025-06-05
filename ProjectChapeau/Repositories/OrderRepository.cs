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

                    List<OrderItem> orderItems =  new List<OrderItem>();
                    

                    OrderStatus orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString());
                    paymentStatus paymentStatus = Enum.Parse<paymentStatus>(reader["payment_status"].ToString());

                    Order order = new Order(
                        (int)reader["order_id"],
                        employee,
                        restaurantTable,
                        orderItems,
                        (DateTime)reader["order_datetime"],
                        orderStatus,
                        paymentStatus
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
                        paymentStatus
                        );

                    orders.Add(order);
                }
                reader.Close();
            }

            return orders;
        }

        public void UpdateOrderStatus(Order order)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Orders SET order_status = @OrderStatus " +
                               "WHERE order_id = @OrderId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderId", order.orderId);
                command.Parameters.AddWithValue("@OrderStatus", order.orderStatus.ToString());

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

            return new OrderItem(menuItemId,orderId, amount,orderLineStatus, comment);
        }
    }
}