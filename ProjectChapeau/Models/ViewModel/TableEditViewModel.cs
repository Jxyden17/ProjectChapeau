using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models.ViewModel
{
    public class TableEditViewModel
    {
       public RestaurantTable table { get; set; }
       public Order? order {  get; set; }

        public IEnumerable<OrderStatus> orderStatusOptions { get; set; }

        public TableEditViewModel(RestaurantTable table, Order? order, IEnumerable<OrderStatus> orderStatusOptions)
        {
            this.table = table;
            this.order = order;
            this.orderStatusOptions = orderStatusOptions;
        }

        public TableEditViewModel()
        {
        }
    }
}
