using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Models.Enums;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class BaseRepository
    {
        protected readonly string? _connectionString;

        protected BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectChapeau");
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
        protected static Employee ReadEmployee(SqlDataReader reader)
        {
            int id = (int)reader["employee_number"];
            string firstname = (string)reader["firstname"];
            string lastname = (string)reader["lastname"];
            string username = (string)reader["username"];
            string password = (string)reader["password"];
            string salt = (string)reader["salt"];
            bool isActive = (bool)reader["is_active"];
            if (!Enum.TryParse<Roles>(reader["role"].ToString(), out Roles employeeRole))
            {
                throw new ArgumentException($"Invalid role {reader["role"]} found in the database.");
            }

            return new Employee(id, firstname, lastname, username, password, isActive, employeeRole, salt);
        }
        protected static RestaurantTable ReadTable(SqlDataReader reader)
        {
            int tableNumber = (int)reader["table_number"];
            bool isOccupied = (bool)reader["is_occupied"];

            return new RestaurantTable(tableNumber, isOccupied);
        }

        protected static Category ReadCategory(SqlDataReader reader)
        {
            // Shortened if-else statement (condition ? true-statement : false-statement)
            int categoryId = (int)reader["category_id"];
            string categoryName = (string)reader["category_name"];
            return new Category(categoryId, categoryName);
        }

        protected static MenuItem ReadMenuItem(SqlDataReader reader)
        {
            int menuItemId = (int)reader["menu_item_id"];
            int? categoryId = reader["category_id"] == DBNull.Value ? null : (int)reader["category_id"];

            Category? category = null;
            // Pattern matching, cast int? categoryId to int id if it has a value and is of type int.
            if (categoryId is int)
            {
                category = ReadCategory(reader);
            }

            string itemName = (string)reader["item_name"];
            string? itemDescription = reader["item_description"] == DBNull.Value ? null : (string)reader["item_description"];
            bool? isAlcoholic = reader["is_alcoholic"] == DBNull.Value ? null : (bool)reader["is_alcoholic"];
            decimal priceExcludingVAT = (decimal)reader["price_excl_vat"];
            int stock = (int)reader["stock"];
            int? prepTime = reader["prep_time"] == DBNull.Value ? null : (int)reader["prep_time"];
            bool isActive = (bool)reader["is_active"];

            return new MenuItem(menuItemId, category, itemName, itemDescription, isAlcoholic, priceExcludingVAT, stock, prepTime, isActive);
        }

        protected static OrderLine ReadOrderLine(SqlDataReader reader)
        {
            int menuItemId = (int)reader["menu_item_id"];
            var menuItem = ReadMenuItem(reader) ?? throw new Exception($"Menu item with ID {menuItemId} does not exist.");
            int amount = (int)reader["amount"];
            string? comment = (string)reader["comment"] as string;
            if (!Enum.TryParse<OrderStatus>(reader["order_line_status"].ToString(), out OrderStatus orderLineStatus))
            {
                throw new ArgumentException($"Invalid order status value {reader["order_line_status"]} found in the database.");
            }

            return new OrderLine(menuItem, amount, comment, orderLineStatus);
        }
    }
}
