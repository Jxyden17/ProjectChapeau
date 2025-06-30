using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;

namespace ProjectChapeau.Repositories
{
    public class BaseRepository
    {
        protected readonly string? _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }

        protected Employee ReadEmployee(SqlDataReader reader)
        {
            int id = (int)reader["employee_number"];
            string firstname = (string)reader["firstname"];
            string lastname = (string)reader["lastname"];
            string username = (string)reader["username"];
            string password = (string)reader["password"];
            string salt = (string)reader["salt"];
            bool isActive = (bool)reader["is_active"];
            Roles employeeRole = Enum.Parse<Roles>(reader["role"].ToString());

            return new Employee(id, firstname, lastname, username, password, isActive, employeeRole, salt);
        }
        protected RestaurantTable ReadTable(SqlDataReader reader)
        {
            int tableNumber = (int)reader["table_number"];
            bool isOccupied = (bool)reader["is_occupied"];

            return new RestaurantTable(tableNumber, isOccupied);
        }

        protected static MenuItem ReadMenuItem(SqlDataReader reader)
        {
            int menuItemId = (int)reader["menu_item_id"];
            // Shortened if-else statement (condition ? true-statement : false-statement)
            Category? category = ReadCategory(reader);
            string itemName = (string)reader["item_name"];
            string? itemDescription = reader["item_description"] == DBNull.Value ? null : (string)reader["item_description"];
            bool? isAlcoholic = reader["is_alcoholic"] == DBNull.Value ? null : (bool)reader["is_alcoholic"];
            decimal price = (decimal)reader["price"];
            int stock = (int)reader["stock"];
            int? prepTime = reader["prep_time"] == DBNull.Value ? null : (int)reader["prep_time"];
            bool isActive = (bool)reader["is_active"];

            return new MenuItem(menuItemId, category, itemName, itemDescription, isAlcoholic, price, stock, prepTime, isActive);
        }
        protected static Category? ReadCategory(SqlDataReader reader)
        {
            if (reader["category_id"] == DBNull.Value)
            {
                return null;
            }
            else
            {
                // Shortened if-else statement (condition ? true-statement : false-statement)
                int categoryId = (int)reader["category_id"];
                string? categoryName = reader["category_name"] == DBNull.Value ? null : (string)reader["category_name"];
                return new Category(categoryId, categoryName);
            }
        }

        protected Order ReadOrder(SqlDataReader reader)
        {
            int orderId = (int)reader["order_id"];
            Employee employee = ReadEmployee(reader);
            RestaurantTable table = ReadTable(reader);
            List<OrderLine>? orderLines = [];
            DateTime orderDateTime = (DateTime)reader["order_datetime"];
            OrderStatus orderStatus = Enum.Parse<OrderStatus>(reader["order_status"].ToString());
            bool isPaid = (bool)reader["is_paid"];
            decimal tipAmount = (decimal)reader["tip_amount"];

            return new Order(orderId, employee, table, orderLines, orderDateTime, orderStatus, isPaid, tipAmount);
        }

        protected OrderLine ReadOrderLine(SqlDataReader reader)
        {
            MenuItem menuItem = ReadMenuItem(reader);
            int amount = (int)reader["amount"];
            string comment = (string)reader["comment"];
            OrderStatus orderLineStatus = Enum.Parse<OrderStatus>(reader["order_line_status"].ToString());

            return new OrderLine(menuItem, amount, comment, orderLineStatus);
        }
    }
}
