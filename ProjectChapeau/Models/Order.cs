namespace ProjectChapeau.Models
{
    public class Order
    {
        public int orderId { get; set; }
        public string employeeNumber { get; set; }  
        public int tableNumber { get; set; }
        public DateTime dateTime { get; set; }
        public string orderStatus { get; set; }
        public string paymentStatus { get; set; }

        public Order(int orderId, string employeeNumber, DateTime dateTime, string orderStatus, string paymentStatus, int tableNumber)
        {
            this.orderId = orderId;
            this.employeeNumber = employeeNumber;
            this.dateTime = dateTime;
            this.orderStatus = orderStatus;
            this.paymentStatus = paymentStatus;
            this.tableNumber = tableNumber;
        }

        public Order() { }
    }
}
