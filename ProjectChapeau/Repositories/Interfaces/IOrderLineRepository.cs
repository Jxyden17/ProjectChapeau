using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IOrderLineRepository
    {
        List<OrderLine> GetAllOrderItemsById(int id);
    }
}
