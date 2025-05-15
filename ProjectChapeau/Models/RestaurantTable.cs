namespace ProjectChapeau.Models
{
    public class RestaurantTable
    {
        public int tableId {  get; set; }
        public bool seatingStatus { get; set; }

        public RestaurantTable(int tableId, bool seatingStatus)
        {
            this.tableId = tableId;
            this.seatingStatus = seatingStatus;
        }
    }
}
