using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Repositories.Interfaces;
namespace ProjectChapeau.Repositories
{
    public class OrderLineRepository : BaseRepository, IOrderLineRepository
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public OrderLineRepository(IConfiguration configuration, IMenuItemRepository menuItemRepository) : base(configuration)
        {
            _menuItemRepository = menuItemRepository;
        }

        public List<OrderLine> GetOrderLinesByOrderId(int orderId)
        {
            List<OrderLine> orderItems = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT order_id, menu_item_id, amount, comment, order_line_status
                                 FROM Order_Line
                                 WHERE order_id = @OrderId;";

                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@OrderId", orderId);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderItems.Add(ReadOrderLine(reader));
                    }
                }
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
