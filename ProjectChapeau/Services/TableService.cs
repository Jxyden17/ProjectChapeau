using ProjectChapeau.Models;
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

        public RestaurantTable GetTableById(int id)
        {
            return _tableRepository.GetById(id);
        }

        public void UpdateTable(RestaurantTable table)
        {
            _tableRepository.UpdateTable(table);
        }
    }
}
