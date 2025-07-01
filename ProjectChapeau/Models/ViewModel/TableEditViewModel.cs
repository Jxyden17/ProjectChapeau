using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models.ViewModel
{
    public class TableEditViewModel
    {
        public RestaurantTable Table { get; set; }
        public Order? Order { get; set; }
        public IEnumerable<OrderStatus> OrderStatusOptions { get; set; }

        public TableEditViewModel()
        {
        }

        public TableEditViewModel(RestaurantTable table, Order? order, IEnumerable<OrderStatus> orderStatusOptions)
        {
            Table = table;
            Order = order;
            OrderStatusOptions = orderStatusOptions;
        }
    }
}
