namespace ProjectChapeau.Models
{
    public class OrderItem
    {
        public int menuItemId { get; set; }
        public int orderId { get; set; } 
        public int amount { get; set; }
        public string comment { get; set; }
        public string orderLineStatus { get; set; }

        public OrderItem(int menuItemId, int orderId, int amount, string comment, string orderLineStatus)
        {
            this.menuItemId = menuItemId;
            this.orderId = orderId;
            this.amount = amount;
            this.comment = comment;
            this.orderLineStatus = orderLineStatus;
        }

        public OrderItem() { }
    }
}
    