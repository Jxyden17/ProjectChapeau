using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models.ViewModel
{
    public class TableEditViewModel
    {
        public int tableID { get; set; }

        public int? orderId { get; set; }
        public bool isOccupied { get; set; }
        public OrderStatus? currentOrderStatus { get; set; }
        public IEnumerable<OrderStatus> orderStatusOptions { get; set; }

        public TableEditViewModel(int tableID, int? orderId, bool isOccupied, OrderStatus? currentOrderStatus, IEnumerable<OrderStatus> orderStatusOptions)
        {
            this.tableID = tableID;
            this.orderId = orderId;
            this.isOccupied = isOccupied;
            this.currentOrderStatus = currentOrderStatus;
            this.orderStatusOptions = orderStatusOptions;
        }

        public TableEditViewModel()
        {
        }
    }
}
