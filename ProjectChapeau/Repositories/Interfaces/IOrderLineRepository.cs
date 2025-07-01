using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IOrderLineRepository
    {
        List<OrderLine> GetOrderLinesByOrderId(int orderId);
        void AddOrderLine(OrderLine orderLine, int orderId);
    }
}
