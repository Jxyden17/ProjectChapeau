using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
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
        public Order? GetOrderById(int orderId)
        {
            return _orderRepository.GetOrderById(orderId);
        }
        public Order? GetLatestOrderForTable(int tableNumber)
        {
            return _orderRepository.GetLatestOrderForTable(tableNumber);
        }
        public void AddOrder(Order order)
        {
            _orderRepository.AddOrder(order);
        }
        public List<Order> GetRunningOrders()
        {
            return _orderRepository.GetRunningOrders();
        }
        public List<Order> GetOrderByPeriod(DateTime startDate, DateTime endDate)
        {
            return _orderRepository.GetOrderByPeriod(startDate, endDate);
        }
        public void UpdateOrderStatus(int? orderId, OrderStatus? newStatus)
        {
            _orderRepository.UpdateOrderStatus(orderId, newStatus);
        }
    }
}