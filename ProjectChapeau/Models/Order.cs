
﻿using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Models

{
    public class Order
    {
        public int orderId { get; set; }

        public Employee employee { get; set; }
        public RestaurantTable table { get; set; }
        public List<OrderItem>? orderItems { get; set; }
        public DateTime datetime { get; set; }
        public OrderStatus orderStatus { get; set; }
        public paymentStatus paymentStatus { get; set; }

        public decimal SalesAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public decimal TipAmount { get; set; }  

        public string Category { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order(int orderId, Employee employee, RestaurantTable table, List<OrderItem> orderItems, DateTime datetime, OrderStatus orderStatus, paymentStatus paymentStatus, decimal salesAmount, decimal incomeAmount, decimal tipAmount)
        {
            this.orderId = orderId;
            this.employee = employee;
            this.table = table;
            this.orderItems = orderItems;
            this.datetime = datetime;
            this.orderStatus = orderStatus;
            this.paymentStatus = paymentStatus;
            SalesAmount = salesAmount;
            IncomeAmount = incomeAmount;
            TipAmount = tipAmount;
        }

        public Order()
        {
        }
    }
}
