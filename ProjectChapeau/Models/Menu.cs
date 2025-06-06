﻿namespace ProjectChapeau.Models
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public List<MenuItem>? MenuItems { get; set; }
        public Menu()
        {
        }
        public Menu(int menuId, string? menuName, List<MenuItem>? menuItems)
        {
            MenuId = menuId;
            MenuName = menuName;
            MenuItems = menuItems;
        }
    }
}
