using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using System.Drawing;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IOrderRepository
    {

        List<Order> GetAllOrders();

        Order GetOrder(int id);
        List<Order> GetRunningOrders();

        void UpdateOrderStatus(int? orderId, OrderStatus? newStatus);
    }
}
