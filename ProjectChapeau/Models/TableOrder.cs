namespace ProjectChapeau.Models
{
    public class TableOrder
    {
        public int tableId { get; set; }
        public string status { get; set; }
        public string cardColor { get; set; }

        public TableOrder(int tableId, string status, string cardColor)
        {
            this.tableId = tableId;
            this.status = status;
            this.cardColor = cardColor;
        }

        public TableOrder()
        {
        }
    }
}
