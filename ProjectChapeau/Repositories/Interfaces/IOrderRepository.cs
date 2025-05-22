using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();

        Order GetOrder(int id);

    }
}
