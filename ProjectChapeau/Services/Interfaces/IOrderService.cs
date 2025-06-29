using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetRunningOrders();
        void UpdateOrderStatus(int? orderId, OrderStatus? newStatus);
    }
}