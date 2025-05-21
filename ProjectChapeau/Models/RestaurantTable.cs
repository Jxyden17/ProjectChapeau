namespace ProjectChapeau.Models
{
    public class RestaurantTable
    {
        public int TableNumber {  get; set; }
        public bool IsOccupied { get; set; }

        public RestaurantTable(int tableNumber, bool isOccupied)
        {
            this.TableNumber = tableNumber;
            this.IsOccupied = isOccupied;
        }

        public RestaurantTable()
        {
        }
    }
}
