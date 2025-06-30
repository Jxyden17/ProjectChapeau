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

        public void AddOrderLine(OrderLine orderLine)
        {
            throw new NotImplementedException();
        }
    }
}
