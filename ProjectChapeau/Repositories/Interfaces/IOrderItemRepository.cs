using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        List<OrderItem> GetAllOrderItemsById(int id);
    }
}
