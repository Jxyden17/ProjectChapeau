using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

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

        private MenuItem ReadMenuItem(SqlDataReader reader)
        {
            string item_name = (string)reader["item_name"];
            decimal price = (decimal)reader["price"];

            return new MenuItem(item_name, price);
        }
    }
}
