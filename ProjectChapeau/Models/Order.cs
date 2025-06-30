using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models

{
    public class Order
    {
        public int OrderId { get; set; }
        public Employee Employee { get; set; }
        public RestaurantTable Table { get; set; }
        public List<OrderLine>? OrderLines { get; set; } = [];
        public DateTime OrderDateTime { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool IsPaid { get; set; }
        public decimal? TipAmount { get; set; }
        public int ItemAmount
        {
            get
            {
                if (OrderLines == null)
                    return 0;
                return OrderLines.Sum(orderLine => (orderLine.Amount));
            }
        }
        public decimal PriceExcludingVAT
        {
            get
            {
                if (OrderLines == null)
                    return 0;
                return OrderLines.Sum(orderLine => (orderLine.PriceExcludingVAT)
                );
            }
        }
        public decimal PriceIncludingVAT
        {
            get
            {
                if (OrderLines == null)
                    return 0;
                return OrderLines.Sum(orderLine => (orderLine.PriceIncludingVAT)
                );
            }
        }
        public decimal VAT
        {
            get { return PriceIncludingVAT - PriceExcludingVAT; }
        }
        public decimal TotalPrice
        {
            get { return PriceIncludingVAT + (TipAmount ?? 0); }
        }

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
