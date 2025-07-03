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

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT menu_id, menu_name
                                 FROM Menu
                                 ORDER BY menu_id;";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int menuId = (int)reader["menu_id"];
                        menuIds.Add(menuId);
                    }
                }
            }
            return menuIds;
        }

        public Menu GetMenuById(int menuId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MCI.menu_id, M.menu_name,
                                     MCI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Contains_Item MCI
                                 JOIN Menu M ON MCI.menu_id = M.menu_id
                                 JOIN Menu_Item MI ON MCI.menu_item_id = MI.menu_item_id
                                 JOIN Category C ON MI.category_id = C.category_id
                                 WHERE MCI.menu_id = @MenuId
                                 ORDER BY MI.category_id, MCI.menu_item_id;";
                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@MenuId", menuId);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        throw new KeyNotFoundException($"No menu found with ID {menuId}");
                    }
                    reader.Read();
                    Menu menu = new Menu(
                        (int)reader["menu_id"],
                        reader["menu_name"] == DBNull.Value ? null : (string)reader["menu_name"],
                        new List<MenuItem>());

                    // First Menu Item
                    menu.MenuItems.Add(ReadMenuItem(reader));
                    while (reader.Read())
                    {
                        MenuItem menuItem = ReadMenuItem(reader);
                        menu.MenuItems.Add(menuItem);
                    }
                    return menu;
                }
            }
        }
    }
}
