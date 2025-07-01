using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;

namespace ProjectChapeau.Services.Interfaces
{
    public interface ITableService
    {
        List<RestaurantTable> GetAllTables();
        RestaurantTable? GetTableByNumber(int tableNumber);
        TableEditViewModel GetTableWithLatestOrder(int tableNumber);
        List<TableWithOrder> GetAllTablesWithLatestOrder();
        void UpdateTableStatus(int tableNumber, bool isOccupied);
    }
}
