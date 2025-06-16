using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;

namespace ProjectChapeau.Services.Interfaces
{
    public interface ITableService
    {
        List<RestaurantTable> GetAllTables();

        RestaurantTable GetTableById(int id);

        void UpdateTableStatus(int tableId, bool isOccupied);

        List<TableViewModel> GetAllTablesWithLatestOrder();

        TableEditViewModel GetTableWithLatestOrderById(int? id);
    }
}
