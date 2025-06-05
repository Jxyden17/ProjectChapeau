namespace ProjectChapeau.Models.ViewModel
{
    public class TableViewModel
    {
        public int tableId { get; set; }
        public string status { get; set; }
        public string cardColor { get; set; }

        public TableViewModel(int tableId, string status, string cardColor)
        {
            this.tableId = tableId;
            this.status = status;
            this.cardColor = cardColor;
        }

        public TableViewModel()
        {
        }
    }
}
