using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models
{
    public class OrderLine
    {
        public MenuItem MenuItem { get; set; } 
        public int Amount { get; set; }
        public string? Comment { get; set; }
        public OrderStatus OrderLineStatus { get; set; }
        public decimal PriceExcludingVAT
        {
            get
            {
                return MenuItem.PriceExcludingVAT * Amount;
            }
        }
        public decimal PriceIncludingVAT
        {
            get
            {
                return MenuItem.PriceIncludingVAT * Amount;
            }
        }

        public OrderLine(MenuItem menuItem, int amount, string? comment, OrderStatus orderLineStatus)
        {
            MenuItem = menuItem;
            Amount = amount;
            Comment = comment;
            OrderLineStatus = orderLineStatus;
        }
    }
}
    