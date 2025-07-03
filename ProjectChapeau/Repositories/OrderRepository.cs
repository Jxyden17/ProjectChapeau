using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services;

namespace ProjectChapeau.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IOrderLineRepository _orderLineRepository;
        public OrderRepository(IConfiguration configuration, IEmployeeRepository employeeRepository, ITableRepository tableRepository, IOrderLineRepository orderLineRepository) : base(configuration)
        {
            _employeeRepository = employeeRepository;
            _tableRepository = tableRepository;
            _orderLineRepository = orderLineRepository;
        }

        // ReadOrder creates an individual Order object, but without any OrderLines! Please use ReadOrderLines() below
        private Order ReadOrder(SqlDataReader reader)
        {
            int orderId = (int)reader["order_id"];
            int employeeNumber = (int)reader["employee_number"];
            int tableNumber = (int)reader["table_number"];
            DateTime orderDateTime = (DateTime)reader["order_datetime"];

            if (!Enum.TryParse<OrderStatus>(reader["order_status"].ToString(), out OrderStatus orderStatus))
            {
                throw new ArgumentException($"Invalid order status value {reader["order_status"]} found in the database.");
            }

            bool isPaid = (bool)reader["is_paid"];
            decimal? tipAmount = (decimal)reader["tip_amount"];

            var employee = _employeeRepository.GetEmployeeByNumber(employeeNumber);
            var table = _tableRepository.GetTableByNumber(tableNumber);

            if (employee == null)
            {
                throw new Exception($"Employee with number {employeeNumber} does not exist");
            }

            if (table == null)
            {

                throw new Exception($"Table with number {tableNumber} does not exist");
            }
            return new Order(orderId, employee, table, new(), orderDateTime, orderStatus, isPaid, tipAmount);
        }

        private static List<OrderLine> ReadOrderLines(SqlDataReader reader)
        {
            List<OrderLine> orderLines = new();

            while (reader.Read())
            {
                if (reader["menu_item_id"] != DBNull.Value)
                {
                    OrderLine orderLine = ReadOrderLine(reader);
                    orderLines.Add(orderLine);
                }
            }

            return orderLines;
        }

        private List<Order> ReadOrders(SqlDataReader reader)
        {
            List<Order> orders = new();
            Order? currentOrder = null;
            List<OrderLine> currentOrderLines = new();

            while (reader.Read())
            {
                // Check if we are encoutering a new order by comparing the order_id
                int currentOrderId = (int)reader["order_id"];
                if (currentOrder == null || currentOrder.OrderId != currentOrderId)
                {
                    // Order_id is different from the previous row
                    if (currentOrder != null)
                    {
                        currentOrder.OrderLines = currentOrderLines;
                        orders.Add(currentOrder);
                    }

                    // Read the new order and initialize the order lines
                    currentOrder = ReadOrder(reader);
                    currentOrderLines = new List<OrderLine>();
                }
                // Add the order_line if it exists
                if (reader["menu_item_id"] != DBNull.Value)
                {
                    OrderLine orderLine = ReadOrderLine(reader);
                    currentOrderLines.Add(orderLine);
                }
            }

            // Add the last order to the list after the loop
            if (currentOrder != null)
            {
                currentOrder.OrderLines = currentOrderLines;
                orders.Add(currentOrder);
            }
            return orders;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> orders = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT O.order_id, O.employee_number, O.table_number, O.order_datetime, O.order_status, O.is_paid, O.tip_amount,
                                     OL.menu_item_id, OL.amount, OL.comment, OL.order_line_status,
                                     MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM [Order] O
                                 LEFT JOIN Order_Line OL on O.order_id = OL.order_id
                                 LEFT JOIN Menu_Item MI on OL.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category C on MI.category_id = C.category_id
                                 ORDER BY O.order_datetime DESC, O.order_id, C.category_id, MI.menu_item_id;";

                SqlCommand command = new(query, connection);
                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    orders = ReadOrders(reader);
                }
            }
            return orders;
        }

        public Order? GetOrderById(int orderId)
        {
            Order? order = null;
            List<OrderLine> orderLines = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT O.order_id, O.employee_number, O.table_number, O.order_datetime, O.order_status, O.is_paid, O.tip_amount,
                                     OL.menu_item_id, OL.amount, OL.comment, OL.order_line_status,
                                     MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM [Order] O
                                 LEFT JOIN Order_Line OL on O.order_id = OL.order_id
                                 LEFT JOIN Menu_Item MI on OL.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category C on MI.category_id = C.category_id
                                 WHERE O.order_id = @OrderId
                                 ORDER BY C.category_id, MI.menu_item_id;";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        order = ReadOrder(reader);
                        orderLines = ReadOrderLines(reader);
                    }
                }
            }
            if (order != null)
            {
                order.OrderLines = orderLines;
            }
            return order;
        }

        public Order? GetLatestOrderForTable(int tableNumber)
        {
            Order? order = null;
            List<OrderLine> orderLines = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT O.order_id, O.employee_number, O.table_number, O.order_datetime, O.order_status, O.is_paid, O.tip_amount,
                                     OL.menu_item_id, OL.amount, OL.comment, OL.order_line_status,
                                     MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM [Order] O
                                 LEFT JOIN Order_Line OL ON O.order_id = OL.order_id
                                 LEFT JOIN Menu_Item MI ON OL.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category C ON MI.category_id = C.category_id
                                 WHERE O.order_id = (SELECT TOP 1 order_id
                                     FROM [Order]
                                     WHERE table_number = @TableNumber
                                     ORDER BY order_datetime DESC)
                                 ORDER BY C.category_id, MI.menu_item_id;";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@TableNumber", tableNumber);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        order = ReadOrder(reader);
                        orderLines = ReadOrderLines(reader);
                    }
                }
            }
            if (order != null)
            {
                order.OrderLines = orderLines;
            }
            return order;
        }
        public void AddOrder(Order order)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"INSERT INTO Orders (employee_number, table_number, order_datetime, order_status, is_paid, tip_amount)
                                 VALUES (@EmployeeNumber, @TableNumber, @OrderDateTime, @OrderStatus, @IsPaid, @TipAmount);
                                 SELECT SCOPE_IDENTITY();";

                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@EmployeeNumber", order.Employee.employeeId);
                command.Parameters.AddWithValue("@TableNumber", order.Table.TableNumber);
                command.Parameters.AddWithValue("@OrderDateTime", order.OrderDateTime);
                command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                command.Parameters.AddWithValue("@IsPaid", order.IsPaid);
                command.Parameters.AddWithValue("@TipAmount", order.TipAmount);

                command.Connection.Open();
                order.OrderId = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public List<Order> GetRunningOrders()
        {
            List<Order> orders = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT O.order_id, O.employee_number, O.table_number, O.order_datetime, O.order_status, O.is_paid, O.tip_amount,
                                     OL.menu_item_id, OL.amount, OL.comment, OL.order_line_status,
                                     MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM [Order] O
                                 LEFT JOIN Order_Line OL on O.order_id = OL.order_id
                                 LEFT JOIN Menu_Item MI on OL.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category C on MI.category_id = C.category_id
                                 WHERE O.order_status IN ('Ordered', 'BeingPrepared', 'ReadyToBeServed', 'Served')
                                 ORDER BY O.order_datetime ASC, O.order_id, C.category_id, MI.menu_item_id;";

                SqlCommand command = new(query, connection);
                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    orders = ReadOrders(reader);
                }
            }
            return orders;
        }

        public void UpdateOrderStatus(int? orderId, OrderStatus? newStatus)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"UPDATE [Order]SET order_status = @OrderStatus
                                 WHERE order_id = @OrderId;";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@OrderId", orderId);
                command.Parameters.AddWithValue("@OrderStatus", newStatus.ToString());

                command.Connection.Open();
                int nrOfRowsAffected = command.ExecuteNonQuery();
                if (nrOfRowsAffected == 0)
                    throw new Exception("No records updated!");
            }
        }

        public List<Order> GetOrderByPeriod(DateTime startDate, DateTime endDate)
        {
            List<Order> orders = [];

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT O.order_id, O.employee_number, O.table_number, O.order_datetime, O.order_status, O.is_paid, O.tip_amount,
                                     OL.menu_item_id, OL.amount, OL.comment, OL.order_line_status,
                                     MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM [Order] O
                                 LEFT JOIN Order_Line OL on O.order_id = OL.order_id
                                 LEFT JOIN Menu_Item MI on OL.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category C on MI.category_id = C.category_id
                                 WHERE O.order_datetime >= @StartDate AND O.order_datetime <= @EndDate 
                                 AND O.is_paid = 1
                                 ORDER BY O.order_datetime ASC, O.order_id, C.category_id, MI.menu_item_id;";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                command.Connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    orders = ReadOrders(reader);
                }
            }
            return orders;
        }
    }
}
