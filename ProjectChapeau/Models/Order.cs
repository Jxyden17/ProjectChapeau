
﻿using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models

{
    public class Order
    {
        public int orderId { get; set; }

        public Employee employee { get; set; }
        public RestaurantTable table { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public DateTime datetime { get; set; }
        public OrderStatus orderStatus{ get; set; }
        public paymentStatus paymentStatus { get; set; }

        public Order(int orderId, Employee employee, RestaurantTable table, List<OrderItem> orderItems, DateTime datetime, OrderStatus orderStatus, paymentStatus paymentStatus)
        {
            this.orderId = orderId;
            this.employee = employee;
            this.table = table;
            OrderItems = orderItems;
            this.datetime = datetime;
            this.orderStatus = orderStatus;
            this.paymentStatus = paymentStatus;
        }
    }
}
