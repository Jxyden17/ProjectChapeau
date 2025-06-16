using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
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
            List<TableOrder> tables = _tableRepository.GetAllTablesWithLatestOrder();
            List<TableViewModel> tableViewModels = new List<TableViewModel>();

            foreach (TableOrder table in tables)
            {
                
                string cardColor = "bg-success text-white";
                string statusText = "Available";

                if (table.OrderId != null && table.OrderStatus != OrderStatus.Completed)
                {
                    cardColor = table.IsOccupied ? "bg-danger text-dark" : "bg-warning text-dark";
                    statusText = $"Order {table.OrderStatus}";
                }
                else if (table.IsOccupied)
                {
                    cardColor = "bg-warning text-dark";
                    statusText = "Occupied";
                }

                tableViewModels.Add(new TableViewModel(table.TableNumber, statusText, cardColor));
            }

            return tableViewModels;
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
