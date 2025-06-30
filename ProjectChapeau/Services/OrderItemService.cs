using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class OrderItemService : IOrderItemService
    {

        private readonly IOrderLineRepository _orderItemRepository;

        public OrderItemService(IOrderLineRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public List<OrderLine> GetAllOrderItemsById(int id)
        {
            return _orderItemRepository.GetAllOrderItemsById(id);
        }
    }
}
