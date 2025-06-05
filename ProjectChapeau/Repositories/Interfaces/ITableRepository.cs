using ProjectChapeau.Models;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface ITableRepository
    {
        List<RestaurantTable> GetAllTables();

        RestaurantTable GetById(int id);

        void UpdateTableStatus(RestaurantTable table);
    }
}
