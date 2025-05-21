using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface ITableService
    {
        List<RestaurantTable> GetAllTables();
    }
}
