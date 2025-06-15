using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models
{
    public class TableOrder
    {
        public int TableNumber { get; set; }
        public bool IsOccupied { get; set; }
        public int? OrderId { get; set; }
        public OrderStatus? OrderStatus { get; set; }

        public TableOrder(int tableNumber, bool isOccupied, int? orderId, OrderStatus? orderStatus)
        {
            TableNumber = tableNumber;
            IsOccupied = isOccupied;
            OrderId = orderId;
            OrderStatus = orderStatus;
        }
    }
}
