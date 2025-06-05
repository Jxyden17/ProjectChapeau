using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetAllOrders()
        {

            return _orderRepository.GetAllOrders();
        }
        public List<Order> GetRunningOrders()
        {
            return _orderRepository.GetRunningOrders();
        }

        public void UpdateOrderStatus(Order order)
        {
            _orderRepository.UpdateOrderStatus(order);
        }
    }
}