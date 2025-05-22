using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models
{
    public class Order
    {
        public int orderId { get; set; }
        public Employee employeeId { get; set; }
        public RestaurantTable tableId { get; set; }
        public DateTime datetime { get; set; }
        public OrderStatus orderStatus{ get; set; }
        public paymentStatus paymentStatus { get; set; }

        public Order(int orderId, Employee employeeId, RestaurantTable tableId, DateTime datetime, OrderStatus orderStatus, paymentStatus paymentStatus)
        {
            this.orderId = orderId;
            this.employeeId = employeeId;
            this.tableId = tableId;
            this.datetime = datetime;
            this.orderStatus = orderStatus;
            this.paymentStatus = paymentStatus;
        }
    }
}
