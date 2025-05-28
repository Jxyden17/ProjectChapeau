using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface ITableService
    {
        List<RestaurantTable> GetAllTables();

        RestaurantTable GetTableById(int id);

        void UpdateTable(RestaurantTable table);
    }
}
