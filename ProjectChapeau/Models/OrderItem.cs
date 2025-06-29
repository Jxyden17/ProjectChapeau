namespace ProjectChapeau.Models
{
    public class OrderItem
    {
        public int MenuItemId { get; set; }
        public int OrderId { get; set; } 
        public int Amount { get; set; }
        public string Comment { get; set; }
        public string OrderLineStatus { get; set; }

        public MenuItem MenuItem { get; set; }

        public OrderItem(int menuItemId, int orderId, int amount, string comment, string orderLineStatus)
        {
            this.MenuItemId = menuItemId;
            this.OrderId = orderId;
            this.Amount = amount;
            this.Comment = comment;
            this.OrderLineStatus = orderLineStatus;
        }
    }
}
    