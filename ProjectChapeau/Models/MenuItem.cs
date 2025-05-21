using Microsoft.AspNetCore.Mvc;

namespace ProjectChapeau.Models
{
    public class MenuItem
    {
		//public int MenuItemId { get; set; }
        public string ItemName { get; set; }
		//public string ItemDescription { get; set; }
        //public bool IsAlcoholic { get; set; }
        public decimal Price { get; set; }
		//public int Stock {  get; set; }
		//public int PrepTime { get; set; }
		//public bool IsActive { get; set; }

		public MenuItem()
		{
		}

        public MenuItem(string item_name, decimal price)
        {
            ItemName = item_name;
            Price = price;
        }
    }

}
