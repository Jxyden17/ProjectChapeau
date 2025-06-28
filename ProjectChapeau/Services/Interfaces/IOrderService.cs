using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetRunningOrders();
        void UpdateOrderStatus(Order order);

    }
}