using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class OrderLineService : IOrderLineService
    {

        private readonly IOrderLineRepository _orderLineRepository;

        public OrderLineService(IOrderLineRepository orderLineRepository)
        {
            _orderLineRepository = orderLineRepository;
        }
        public List<OrderLine> GetOrderLinesByOrderId(int orderId)
        {
            return _orderLineRepository.GetOrderLinesByOrderId(orderId);
        }
        public void AddOrderLine(OrderLine orderLine)
        {
            _orderLineRepository.AddOrderLine(orderLine);
        }
    }
}
