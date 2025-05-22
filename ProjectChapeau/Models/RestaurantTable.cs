namespace ProjectChapeau.Models
{
    public class RestaurantTable
    {
        public int TableNumber {  get; set; }

        public RestaurantTable(int tableNumber)
        {
            this.TableNumber = tableNumber;
        }

        public RestaurantTable()
        {
        }
    }
}
