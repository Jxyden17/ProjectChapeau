using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderLineService
    {
        List<OrderLine> GetOrderLinesByOrderId(int orderId);
        void AddOrderLine(OrderLine orderLine);
    }
}
