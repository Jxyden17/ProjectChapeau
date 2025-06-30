using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models
{
    public class OrderLine
    {
        public MenuItem MenuItem { get; set; } 
        public int Amount { get; set; }
        public string? Comment { get; set; }
        public OrderStatus OrderLineStatus { get; set; }

        public OrderLine(MenuItem menuItem, int amount, string? comment, OrderStatus orderLineStatus)
        {
            MenuItem = menuItem;
            Amount = amount;
            Comment = comment;
            OrderLineStatus = orderLineStatus;
        }
    }
}
    