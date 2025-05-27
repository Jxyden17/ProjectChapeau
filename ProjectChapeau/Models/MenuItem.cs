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

        public string ImageURL => MenuItemId switch
        {
            2 => "steak-tartare.jpg",
            7 => "pheasant-pate.webp",
            9 => "provencal-fish-soup-aged-cheese.jpg",
            12 => "stewed-venison.jpg",
            13 => "pan-fried-cod.jpg",
            15 => "linguini-mushroom.jpg",
            16 => "white-chocolate-tart.jpg",
            17 => "madeleine.jpg",
            19 => "3-cheeses.webp",
            20 => "veal-tartare.jpg",
            22 => "crab-salmon-cookies.jpg",
            23 => "provencal-fish-soup-no-cheese.jpg",
            24 => "pheasant-consomme.jpg",
            25 => "pan-seared-cod.jpg",
            26 => "pan-fried-beef-tenderloin.jpg",
            27 => "venison-steak.jpg",
            28 => "cafe-surprise.jpg",
            30 => "cherry-baby.webp",
            31 => "port-e-fromage.jpg",
            32 => "spa-red.png",
            35 => "spa-green.webp",
            37 => "coca-cola.webp",
            38 => "coca-cola-light.jpg",
            40 => "sisi.webp",
            42 => "tonic.jpg",
            44 => "bitter-lemon.jpg",
            46 => "hertog-jan.webp",
            48 => "duvel.webp",
            51 => "kriek.png",
            52 => "leffe.webp",
            53 => "white-wine-bottle.webp",
            55 => "white-wine-glass.jpg",
            57 => "red-wine-bottle.webp",
            58 => "red-wine-glass.jpg",
            59 => "champagne.jpg",
            60 => "young-jenever.webp",
            61 => "whiskey.webp",
            62 => "rum.jpg",
            63 => "vieux.jpg",
            65 => "berenburg.jpg",
            66 => "coffee.jpg",
            67 => "cappuccino.webp",
            68 => "espresso.jpg",
            70 => "tea.jpg",
            _ => "steak-tartare.jpg"
        };

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
