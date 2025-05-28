

using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
namespace ProjectChapeau.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {

        private readonly string? _connectionString;

        public OrderItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }
        public List<OrderItem> GetAllOrderItemsById(int id)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"SELECT 
                            order_id,
                            menu_item_id,
                            order_line_status,
                            comment,
                            amount
                         FROM OrderItem
                         WHERE order_id = @OrderId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderId", id);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem orderItem = ReadOrderItem(reader);
                    orderItems.Add(orderItem);
                }

                reader.Close();
            }

            return orderItems;
        }


        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            int orderId = (int)reader["order_id"];
            int menuItemId = (int)reader["menu_item_id"];
            string orderLineStatus = (string)reader["order_line_status"];
            string comment = (string)reader["comment"];
            int amount = (int)reader["amount"];

            return new OrderItem(menuItemId, orderId, amount, comment, orderLineStatus);
        }
    }
}
