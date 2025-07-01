namespace ProjectChapeau.Models.ViewModel
{
    public class TableWithOrder
    {
        public RestaurantTable Table { get; set; }
        public Order? Order { get; set; }

        public TableWithOrder()
        { }

        public TableWithOrder(RestaurantTable table, Order? order)
        {
            Table = table;
            Order = order;
        }
    }
}
