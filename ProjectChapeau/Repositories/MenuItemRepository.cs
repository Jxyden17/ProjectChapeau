using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace ProjectChapeau.Repositories
{
    public class MenuItemRepository : ConnectionDatabase, IMenuItemRepository
    {
        public MenuItemRepository(IConfiguration configuration) : base(configuration)
        { }
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
			List<MenuItem> menuItems = new List<MenuItem>();

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				string query = "SELECT MI.*, C.category_name FROM Menu_Item AS MI LEFT JOIN Category AS C ON C.category_id = MI.category_id ORDER BY item_name";
				SqlCommand command = new SqlCommand(query, connection);

				connection.Open();
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
                string query = @"SELECT MI.*, C.category_name
                                 FROM Menu_Item AS MI
                                 LEFT JOIN Category AS C ON C.category_id = MI.category_id
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

        public List<MenuItem> GetCategory(int categoryId)
        {
            List<MenuItem> category = new();

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT M.item_name, M.price, M.stock, m.is_active
                                 FROM Category AS C
                                 JOIN Menu_Item AS M
                                     ON M.category_id = C.category_id
                                 WHERE C.category_id = @CategoryId;";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@CategoryId", categoryId);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem menuItem = ReadMenuItem(reader);
                    category.Add(menuItem);
                }
                reader.Close();
            }
            return category;
        }

		public List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId)
		{
			List<MenuItem> menuItemList = new List<MenuItem>();

			using (SqlConnection connection = new(_connectionString))
			{
				string query = @"SELECT MI.*, C.category_name
								FROM Menu_Item AS MI
                                LEFT JOIN Category AS C ON C.category_id = MI.category_id
                                LEFT JOIN Menu_Contains_Item AS MCI ON MI.menu_item_id = MCI.menu_item_id
								WHERE (@menuId IS NULL OR MCI.menu_id = @menuId)
								AND (@categoryId IS NULL OR MI.category_id = @categoryId)
								ORDER BY MI.item_name";

				SqlCommand command = new(query, connection);
				command.Parameters.AddWithValue("@menuId", (object?)menuId ?? DBNull.Value);
				command.Parameters.AddWithValue("@categoryId", (object?)categoryId ?? DBNull.Value);

				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					menuItemList.Add(ReadMenuItem(reader));
				}
				reader.Close();
			}
			return menuItemList;
		}

			public void AddMenuItem(MenuItem menuItem)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"INSERT INTO Menu_Item (item_name, price, stock, is_active)
                                 VALUES (@item_name, @price, @stock, @is_active);";

                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@item_name", menuItem.ItemName);
                command.Parameters.AddWithValue("@price", menuItem.Price);
                command.Parameters.AddWithValue("@stock", menuItem.Stock);
                command.Parameters.AddWithValue("@is_active", menuItem.IsActive);


                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"UPDATE Menu_Item
                                 SET item_name = @item_name, price = @price, stock = @stock
                                 WHERE menu_item_id = @menu_item_id";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@menu_item_id", menuItem.MenuItemId);
                command.Parameters.AddWithValue("@item_name", menuItem.ItemName);
                command.Parameters.AddWithValue("@price", menuItem.Price);
                command.Parameters.AddWithValue("@stock", menuItem.Stock);
                command.Parameters.AddWithValue("@category_id", menuItem.Category.CategoryId);
                command.Parameters.AddWithValue("@menu_id", menuItem.Category.CategoryName);
                command.Parameters.AddWithValue("@is_active", menuItem.IsActive);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeactivateMenuItem(int menuItemId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = "UPDATE Menu_Item SET is_active = 0 WHERE menu_item_id = @menu_item_id";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@menu_item_id", menuItemId);

                connection.Open();
                command.ExecuteNonQuery();
            }
		}

        public void ActivateMenuItem(int menuItemId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string query = "UPDATE Menu_Item SET is_active = 1 WHERE menu_item_id = @menu_item_id";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@menu_item_id", menuItemId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
	}
}

