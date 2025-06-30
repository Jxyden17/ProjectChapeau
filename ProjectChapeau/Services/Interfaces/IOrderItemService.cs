using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IOrderItemService
    {
        List<OrderLine> GetAllOrderItemsById(int id);
    }
}
