using Microsoft.AspNetCore.Mvc;

namespace ProjectChapeau.Models
{
    public class MenuItem
    {
		public int MenuItemId { get; set; }
        public string ItemName { get; set; }
		//public string ItemDescription { get; set; }
        //public bool IsAlcoholic { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        //public int PrepTime { get; set; }
        public bool IsActive { get; set; } = true;

        public int CategoryId { get; set; }
        public int MenuId { get; set; }


        public MenuItem()
		{
		}

        public MenuItem(int menuItemId, string item_name, decimal price, int stock, int categoryId, int menuId)
		{
			MenuItemId = menuItemId;
			ItemName = item_name;
			Price = price;
			Stock = stock;
			CategoryId = categoryId;
			MenuId = menuId;
		}
	}

}
