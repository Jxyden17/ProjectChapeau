namespace ProjectChapeau.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public Category? Category { get; set; }
        public string ItemName { get; set; }
        public string? ItemDescription { get; set; }
        public bool? IsAlcoholic { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int? PrepTime { get; set; }
        public bool IsActive { get; set; }

        // An empty constructor is needed when a new MenuItem object is created in the controller!
        public MenuItem()
        {
            ItemName = string.Empty;
        }

        public MenuItem(int menuItemId, Category? category, string itemName, string? itemDescription, bool? isAlcoholic, decimal price, int stock, int? prepTime, bool isActive)
        {
            MenuItemId = menuItemId;
            Category = category;
            ItemName = itemName;
            ItemDescription = itemDescription;
            IsAlcoholic = isAlcoholic;
            Price = price;
            Stock = stock;
            PrepTime = prepTime;
            IsActive = isActive;
        }
    }
}
