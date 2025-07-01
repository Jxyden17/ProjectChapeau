using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order? GetOrderById(int orderId);
        Order? GetLatestOrderForTable(int tableNumber);
        void AddOrder(Order order);
        List<Order> GetRunningOrders();
        List<Order> GetOrderByPeriod(DateTime startDate, DateTime endDate);
        void UpdateOrderStatus(int? orderId, OrderStatus? newStatus);
    }
}