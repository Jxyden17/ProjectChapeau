using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace ProjectChapeau.Repositories
{
    public class MenuItemRepository : BaseRepository, IMenuItemRepository
    {
        private readonly ICategoryRepository _categoryRepository;
        public MenuItemRepository(IConfiguration configuration, ICategoryRepository categoryRepository) : base(configuration) => _categoryRepository = categoryRepository;

        public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Item
                                 LEFT JOIN Category C on MI.catgory_id = C.category_id
                                 ORDER BY MI.item_name";
                SqlCommand command = new(query, connection);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    MenuItem menuItem = ReadMenuItem(reader);
                    menuItems.Add(menuItem);
                }
            }
            return menuItems;
        }

        public MenuItem GetMenuItemById(int menuItemId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Item MI
                                 LEFT JOIN Category C on MI.category_id = C.category_id
                                 WHERE MI.menu_item_id = @MenuItemId;";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@MenuItemId", menuItemId);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (!reader.Read())
                    {
                        throw new KeyNotFoundException($"No menu item found in the database with ID {menuItemId}.");
                    }
                    return ReadMenuItem(reader);
                }
            }
        }
        public List<MenuItem> GetMenuItemsByMenuId(int menuId)
        {
            List<MenuItem> menuItems = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MCI.menu_id, MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Item MI
                                 LEFT JOIN Menu_Contains_Item MCI ON MI.menu_item_id = MCI.menu_item_id
                                 LEFT JOIN Category C ON MI.category_id = C.category_id
                                 WHERE MCI.menu_id = @MenuId
                                 ORDER BY MI.category_id, MI.menu_item_id;";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@MenuId", menuId);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItems.Add(ReadMenuItem(reader));
                    }
                }
            }
            return menuItems;
        }

        public List<MenuItem> GetMenuItemsWithoutMenuId()
        {
            List<MenuItem> menuItems = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
                                 FROM Menu_Item MI
                                 LEFT JOIN Menu_Contains_Item MCI ON MI.menu_item_id = MCI.menu_item_id
                                 LEFT JOIN Category C ON MI.category_id = C.category_id
                                 WHERE MCI.menu_item_id IS NULL
                                 ORDER BY MI.category_id, MI.menu_item_id";
                SqlCommand command = new(query, connection);

                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItems.Add(ReadMenuItem(reader));
                    }
                }
            }
            return menuItems;
        }

        public List<MenuItem> GetFilteredMenuItems(int? menuId, int? categoryId)
        {
            List<MenuItem> menuItemList = new();

            using (SqlConnection connection = CreateConnection())
            {
                string query = @"SELECT MI.menu_item_id, MI.category_id, C.category_name, MI.item_name, MI.item_description, MI.is_alcoholic, MI.price_excl_vat, MI.stock, MI.prep_time, MI.is_active
								FROM Menu_Item MI
                                LEFT JOIN Menu_Contains_Item MCI ON MI.menu_item_id = MCI.menu_item_id
                                LEFT JOIN Category C ON C.category_id = MI.category_id
								WHERE (@MenuId IS NULL OR MCI.menu_id = @menuId)
								AND (@CategoryId IS NULL OR MI.category_id = @categoryId)
								ORDER BY MI.item_name";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@MenuId", (object?)menuId ?? DBNull.Value);
                command.Parameters.AddWithValue("@CategoryId", (object?)categoryId ?? DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menuItemList.Add(ReadMenuItem(reader));
                    }
                }
            }
            return menuItemList;
        }

        public void AddMenuItem(MenuItem menuItem)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"INSERT INTO Menu_Item (item_name, price_excl_vat, stock, is_active)
                                 VALUES (@ItemName, @PriceExcludingVAT, @Stock, @IsActive);";

                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@ItemName", menuItem.ItemName);
                command.Parameters.AddWithValue("@PriceExcludingVAT", menuItem.PriceExcludingVAT);
                command.Parameters.AddWithValue("@Stock", menuItem.Stock);
                command.Parameters.AddWithValue("@IsActive", menuItem.IsActive);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"UPDATE Menu_Item
                                 SET item_name = @ItemName, price_excl_vat = @PriceExcludingVAT, stock = @Stock
                                 WHERE menu_item_id = @MenuItemId";

                SqlCommand command = new(query, connection);
                command.Parameters.AddWithValue("@MenuItemId", menuItem.MenuItemId);
                command.Parameters.AddWithValue("@ItemName", menuItem.ItemName);
                command.Parameters.AddWithValue("@PriceExcludingVAT", menuItem.PriceExcludingVAT);
                command.Parameters.AddWithValue("@Stock", menuItem.Stock);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeactivateMenuItem(int menuItemId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = "UPDATE Menu_Item SET is_active = 0 WHERE menu_item_id = @MenuItemId";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@MenuItemId", menuItemId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ActivateMenuItem(int menuItemId)
        {
            using (SqlConnection connection = CreateConnection())
            {
                string query = @"UPDATE Menu_Item SET is_active = 1
                                 WHERE menu_item_id = @MenuItemId";
                SqlCommand command = new(query, connection);

                command.Parameters.AddWithValue("@MenuItemId", menuItemId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}