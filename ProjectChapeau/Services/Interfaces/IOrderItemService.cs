using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderItemService
    {
        List<OrderItem> GetAllOrderItemsById(int id);
    }
}
