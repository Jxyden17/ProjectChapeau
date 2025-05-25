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


		public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT item_name, price FROM Menu_Item ORDER BY item_name";
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



		public List<MenuItem> GetCategory(int categoryId)
		{
			List<MenuItem> category = new();

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
			{
                // 2. Create an SQL command with a query
                string query = @"SELECT M.item_name, M.price
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

		private MenuItem ReadMenuItem(SqlDataReader reader)
        {
            int menuItemId = (int)reader["menu_item_id"];
            string item_name = (string)reader["item_name"];
            decimal price = (decimal)reader["price"];

            return new MenuItem(menuItemId, item_name, price, 0, 0);
        }

        public MenuItem? GetMenuItemById(int menuItemId)
        {
            MenuItem? menuItem = null;
            using (SqlConnection connection = new(_connectionString))
            {
                string query = "SELECT menu_item_id, item_name, price FROM Menu_Item WHERE menu_item_id = @menu_item_id";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@menu_item_id", menuItemId);

                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    menuItem = ReadMenuItem(reader);
                }
            }
            return menuItem;
        }

		public List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId)
		{
			List<MenuItem> menuItemList = new List<MenuItem>();

            using (SqlConnection connection = new(_connectionString))
            {
                string query = @"SELECT menu_item_id, item_name, price
                                 FROM Menu_Item
                                 WHERE (@menuId IS NULL OR menu_id = @menuId)
                                 AND (@categoryId IS NULL OR category_id = @categoryId)
                                 ORDER BY item_name";

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
	}
}
