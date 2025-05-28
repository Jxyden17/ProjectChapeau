using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = _orderRepository.GetAllOrders();

            foreach (Order order in orders)
            {
                order.orderItems = _orderItemRepository.GetAllOrderItemsById(order.orderId);
            }
            return orders;
        }
        public List<Order> GetRunningOrders()
        {
            return _orderRepository.GetRunningOrders();
        }
    }
}