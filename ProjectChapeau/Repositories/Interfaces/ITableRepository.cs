using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;

namespace ProjectChapeau.Repositories.Interfaces
{
    public interface ITableRepository
    {
        List<RestaurantTable> GetAllTables();
        List<int> GetAllTableNumbers();
        RestaurantTable? GetTableByNumber(int tableNumber);
        void UpdateTableStatus(int tableId, bool isOccupied);
    }
}
 