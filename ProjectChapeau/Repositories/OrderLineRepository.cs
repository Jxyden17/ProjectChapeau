using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
namespace ProjectChapeau.Repositories
{
    public class OrderLineRepository : BaseRepository, IOrderLineRepository
    {
        public OrderLineRepository(IConfiguration configuration) : base(configuration)
        { }

        public List<OrderLine> GetAllOrderItemsById(int id)
        {
            List<OrderLine> orderItems = [];

            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"SELECT 
                            order_id,
                            menu_item_id,
                            order_line_status,
                            comment,
                            amount
                         FROM order_item
                         WHERE order_id = @OrderId";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@OrderId", id);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    OrderLine orderItem = ReadOrderLine(reader);
                    orderItems.Add(orderItem);
                }

                reader.Close();
            }

            return orderItems;
        }

        public void AddOrderLine(OrderLine orderLine, int orderId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Order_Item (order_id, menu_item_id, amount, comment, order_line_status)
                                 VALUES (@OrderId, @MenuItemId, @Amount, @Comment, @OrderLineStatus);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@MenuItemId", orderLine.MenuItem.MenuItemId);
                command.Parameters.AddWithValue("@Amount", orderLine.Amount);
                command.Parameters.AddWithValue("@Comment", orderLine.Comment);
                command.Parameters.AddWithValue("@OrderLineStatus", orderLine.OrderLineStatus);

                command.Connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }
}
