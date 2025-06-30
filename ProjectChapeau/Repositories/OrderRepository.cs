using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services;

namespace ProjectChapeau.Repositories
{
    public class OrderRepository : BaseRepository,  IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration) { }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = [];

            using (SqlConnection connection = new(_connectionString))
            {
                string query = @" SELECT 
                o.order_id,
                o.order_datetime,
                o.order_status,
                o.is_paid,
                e.employee_number, e.firstname, e.lastname, e.username, e.password, e.salt, e.is_active, e.role,
                rt.table_number, rt.is_occupied
                FROM Orders o
                JOIN Employees e ON o.employee_number = e.employee_number
                JOIN RESTAURANT_TABLE rt ON o.table_number = rt.table_number;";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = ReadOrder(reader);

                    orders.Add(order);
                }
                reader.Close();
            }

            return orders;
        }

        public Order GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetRunningOrders()
        {
            List<Order> orders = [];

            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"SELECT 
                o.*,
                o.tip_amount,
                e.employee_number, e.firstname, e.lastname, e.username, e.password, e.salt, e.is_active, e.role,
                rt.table_number, rt.is_occupied
                FROM Orders o
                JOIN Employees e ON o.employee_number = e.employee_number
                JOIN RESTAURANT_TABLE rt ON o.table_number = rt.table_number
    
                WHERE o.order_status = 'BeingPrepared'
                ORDER BY o.order_datetime ASC;";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Order order = ReadOrder(reader);
                    orders.Add(order);
                }
                reader.Close();
            }

            return orders;
        }

        public void UpdateOrderStatus(int? orderId, OrderStatus? newStatus)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"UPDATE Orders SET order_status = @OrderStatus
                                 WHERE order_id = @OrderId";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@OrderStatus", newStatus.ToString());

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public List<Order> GetOrderByPeriod(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = [];

            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"SELECT o.*,
                                 mi.menu_item_id, mi.price,
                                 m.menu_name AS item_category, 
                                 (mi.price * oi.amount) AS sales_amount,
                                 ((mi.price * oi.amount) + o.tip_amount) AS income_amount
                                 FROM Orders o
                                 JOIN order_item oi ON o.order_id = oi.order_id
                                 JOIN Menu_Item mi ON oi.menu_item_id = mi.menu_item_id
                                 JOIN Menu_Contains_Item mci ON mi.menu_item_id = mci.menu_item_id
                                 JOIN Menu m ON mci.menu_id = m.menu_id
                                 WHERE o.order_datetime >= @StartDate AND o.order_datetime <= @EndDate 
                                 AND o.is_paid = 1";

                SqlCommand command = new(query, connection); 
                command.Parameters.AddWithValue("@StartDate",startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(ReadOrder(reader));
                }
            }
            return orders;
        }
    }
}