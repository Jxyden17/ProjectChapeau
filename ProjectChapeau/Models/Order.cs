using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models

{
    public class Order
    {
        public int OrderId { get; set; }
        public Employee Employee { get; set; }
        public RestaurantTable Table { get; set; }
        public List<OrderLine>? OrderLines { get; set; } = new();
        public DateTime OrderDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsPaid { get; set; }
        public decimal SubtotalAmount
        {
            get
            {
                if (OrderLines == null)
                    return 0;

                return OrderLines.Sum(line => (line?.MenuItem?.Price ?? 0) * line.Amount
                );
            }
        }
        public decimal TotalAmount
        {
            get
            {
                return SubtotalAmount + (TipAmount ?? 0);
            }
        }
        public decimal? TipAmount { get; set; }

        public Order()
        {
        }

        public Order(int orderId, Employee employee, RestaurantTable table, List<OrderLine>? orderLines, DateTime orderDateTime, OrderStatus orderStatus, bool isPaid, decimal? tipAmount)
        {
            OrderId = orderId;
            Employee = employee;
            Table = table;
            OrderLines = orderLines;
            OrderDateTime = orderDateTime;
            OrderStatus = orderStatus;
            IsPaid = isPaid;
            TipAmount = tipAmount;
        }
    }
}
