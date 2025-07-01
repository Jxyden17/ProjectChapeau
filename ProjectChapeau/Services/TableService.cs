using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Repositories;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IOrderRepository _orderRepository;

        public TableService(ITableRepository tableRepository, IOrderRepository orderRepository)
        {
            _tableRepository = tableRepository;
            _orderRepository = orderRepository;
        }

        public List<RestaurantTable> GetAllTables()
        {
            return _tableRepository.GetAllTables();
        }
        public RestaurantTable? GetTableByNumber(int tableNumber)
        {
            return _tableRepository.GetTableByNumber(tableNumber);
        }
        public TableEditViewModel GetTableWithLatestOrder(int tableNumber)
        {
            RestaurantTable? table = _tableRepository.GetTableByNumber(tableNumber);
            Order? order = _orderRepository.GetLatestOrderForTable(tableNumber);
            IEnumerable<OrderStatus> statusOptions = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();

            return new TableEditViewModel(table, order, statusOptions);
        }
        public List<TableWithOrder> GetAllTablesWithLatestOrder()
        {
            List<TableWithOrder> tablesWithOrder = new();

            List<RestaurantTable> tables = _tableRepository.GetAllTables();

            foreach (RestaurantTable table in tables)
            {
                Order? latestOrder = _orderRepository.GetLatestOrderForTable(table.TableNumber);
                tablesWithOrder.Add(new TableWithOrder
                {
                    Table = table,
                    Order = latestOrder
                });
            }
            return tablesWithOrder;
        }

        public void UpdateTableStatus(int tableNumber, bool isOccupied)
        {
            _tableRepository.UpdateTableStatus(tableNumber, isOccupied);
        }
    }
}
