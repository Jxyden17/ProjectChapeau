using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using System.Diagnostics;

namespace ProjectChapeau.Repositories
{
    public class MenuItemsRepository : IMenuItemsRepository
    {
        private readonly string? _connectionString;

        public MenuItemsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ProjectDatabase");
        }

        private static MenuItem ReadMenuItem(SqlDataReader reader)
        {
            int menuItemId = (int)reader["menu_item_id"];
            // Shortened if-else statement (condition ? true-statement : false-statement)
            Category? category = reader["category_id"] == DBNull.Value || reader["category_name"] == DBNull.Value ? null : new Category((int)reader["category_id"], (string)reader["category_name"]);
            string itemName = (string)reader["item_name"];
            string? itemDescription = reader["item_description"] == DBNull.Value ? null : (string)reader["item_description"];
            bool? isAlcoholic = reader["is_alcoholic"] == DBNull.Value ? null : (bool)reader["is_alcoholic"];
            decimal price = (decimal)reader["price"];
            int stock = (int)reader["stock"];
            int? prepTime = reader["prep_time"] == DBNull.Value ? null : (int)reader["prep_time"];
            bool isActive = (bool)reader["is_active"];


            return new MenuItem(menuItemId, category, itemName, itemDescription, isAlcoholic, price, stock, prepTime, isActive);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = [];

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT MCI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Contains_Item AS MCI
                                 INNER JOIN Menu_Item AS MI ON MI.menu_item_id = MCI.menu_item_id
                                 INNER JOIN Category AS C ON C.category_id = MI.category_id
                                 ORDER BY MCI.menu_item_id;";
                SqlCommand command = new(query, connection);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem menuItem = ReadMenuItem(reader);
                    menuItems.Add(menuItem);
                }
                reader.Close();
            }
            return menuItems;
        }

        public List<MenuItem> GetMenuItemsByMenu(int menuId)
        {
            List<MenuItem> menuItems = [];

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT MCI.menu_id, MCI.menu_item_id, M.menu_name, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Contains_Item AS MCI
                                 INNER JOIN Menu AS M ON M.menu_id = MCI.menu_id
                                 INNER JOIN Menu_Item AS MI ON MI.menu_item_id = MCI.menu_item_id
                                 INNER JOIN Category AS C ON C.category_id = MI.category_id
                                 WHERE MCI.menu_id LIKE @MenuId
                                 ORDER BY MI.category_id;";
                SqlCommand command = new(query, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@MenuId", menuId);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem menuItem = ReadMenuItem(reader);
                    menuItems.Add(menuItem);
                }
                reader.Close();
            }
            return menuItems;
        }
        public MenuItem? GetMenuItemById(int menuItemId)
        {
            MenuItem? menuItem = null;

            // 1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Item AS MI
                                 INNER JOIN Category AS C ON C.category_id = MI.category_id
                                 WHERE MI.menu_item_id = @MenuItemId;";
                SqlCommand command = new(query, connection);

                // Add parameters to prevent SQL injection
                command.Parameters.AddWithValue("@MenuItemId", menuItemId);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    menuItem = ReadMenuItem(reader);
                }
                reader.Close();
            }
            return menuItem;
        }
    }
}
