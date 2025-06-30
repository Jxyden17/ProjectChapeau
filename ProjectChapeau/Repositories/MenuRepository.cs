using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Repositories
{
    public class MenuRepository : BaseRepository, IMenuRepository
    {
        public MenuRepository(IConfiguration configuration) : base(configuration)
        { }
        public List<int> GetAllMenuIds()
        {
            List<int> menuIds = new();

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT *
                                 FROM Menu
                                 ORDER BY menu_id;";
                SqlCommand command = new(query, connection);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int menuId = (int)reader["menu_id"];
                    menuIds.Add(menuId);
                }
                reader.Close();
            }

            return menuIds;
        }

        public Menu GetMenuById(int menuId)
        {
            Menu menu = new();

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT MCI.menu_id, M.menu_name, MI.*, C.category_name
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
                    if (string.IsNullOrEmpty(menu.MenuName))
                    {
                        menu.MenuName = reader["menu_name"].ToString();
                    }
                    MenuItem menuItem = ReadMenuItem(reader);
                    menu.MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            menu.MenuId = menuId;

            return menu;
        }

        public Menu GetMenuItemsWithoutDefinedMenu(string menuName)
        {
            Menu menu = new();

            //1. Create an SQL connection with a connection string
            using (SqlConnection connection = new(_connectionString))
            {
                // 2. Create an SQL command with a query
                string query = @"SELECT MI.*, C.category_name
                                 FROM Menu_Item AS MI
                                 LEFT JOIN Menu_Contains_Item AS MCI ON MCI.menu_item_id = MI.menu_item_id
                                 LEFT JOIN Category AS C ON C.category_id = MI.category_id
                                 WHERE MCI.menu_item_id IS NULL
                                 ORDER BY MI.category_id;";
                SqlCommand command = new(query, connection);

                // 3. Open the SQL connection
                command.Connection.Open();

                // 4. Execute SQL command
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MenuItem menuItem = ReadMenuItem(reader);
                    menu.MenuItems.Add(menuItem);
                }
                reader.Close();
            }
            menu.MenuName = menuName;
            menu.MenuId = 0;

            return menu;
        }
    }
}
