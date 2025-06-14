using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;

        public TableService(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public List<RestaurantTable> GetAllTables()
        {
            return _tableRepository.GetAllTables();
        }

        public List<TableViewModel> GetAllTablesWithLatestOrder()
        {
            return _tableRepository.GetAllTablesWithLatestOrder();
        }

        public RestaurantTable GetTableById(int id)
        {
            return _tableRepository.GetTableById(id);
        }

        public TableEditViewModel GetTableWithLatestOrderById(int? id)
        {
            return _tableRepository.GetTableWithLatestOrderById(id);
        }

        public void UpdateTableStatus(int tableId, bool isOccupied)
        {
            _tableRepository.UpdateTableStatus(tableId, isOccupied);
        }
    }
}
