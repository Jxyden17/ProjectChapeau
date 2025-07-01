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

        public void AddOrderLine(OrderLine orderLine)
        {
            throw new NotImplementedException();
        }
    }
}
