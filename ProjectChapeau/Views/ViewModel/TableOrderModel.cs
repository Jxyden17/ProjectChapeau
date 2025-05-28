using ProjectChapeau.Models;

namespace ProjectChapeau.Views.ViewModel
{
    public class TableOrderModel
    {
        public List<RestaurantTable> restaurantTables { get; set; }
        public List<Order> Orders { get; set; }

        public TableOrderModel(List<RestaurantTable> restaurantTables, List<Order> orders)
        {
            this.restaurantTables = restaurantTables;
            Orders = orders;
        }

        public TableOrderModel()
        {
        }
    }
}
