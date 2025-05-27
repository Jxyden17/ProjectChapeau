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

        public string ImageURL => ItemName switch
        {
            "Steak tartare with truffle mayonnaise" => "steak-tartare.jpg",
            "Pheasant pâté with Monégasque onions" => "pheasant-pate.webp",
            "Provençal fish soup with rouille, aged cheese and croutons" => "provencal-fish-soup-aged-cheese.jpg",
            "Stewed venison with red cabbage" => "stewed-venison.jpg",
            "Pan-fried cod with curry sabayon" => "pan-fried-cod.jpg",
            "Linguini with mushroom sauce" => "linguini-mushroom.jpg",
            "White chocolate and speculaas tart with mandarin" => "white-chocolate-tart.jpg",
            "Fresh madeleine with fig compote and Grand Marnier crème pâtissière" => "madeleine.jpg",
            "Three types of farmhouse cheese with rye raisin bread" => "3-cheeses.webp",
            "Veal tartare with tuna mayonnaise and fried mussels" => "veal-tartare.avif",
            "Crab and salmon cakes with sweet chili sauce" => "crab-salmon-cookies.jpg",
            "Provençal fish soup with rouille and croutons" => "provencal-fish-soup-no-cheese.jpg",
            "Pheasant consommé with spring onion and fresh herbs" => "pheasant-consomme.jpg",
            "Pan-seared cod loin fillet with curry sabayon" => "pan-seared-cod.jpg",
            "Pan-fried beef tenderloin with veal jus and pink peppercorns" => "pan-fried-beef-tenderloin.jpg",
            "Venison steak with its own stew and red cabbage" => "venison-steak.jpg",
            "Café Surprise" => "cafe-surprise.jpg",
            "Cherry Baby" => "cherry-baby.webp",
            "Port e Fromage" => "port-e-fromage.jpg",
            "Spa Red" => "spa-red.png",
            "Spa Green" => "spa-green.webp",
            "Coca Cola" => "coca-cola.webp",
            "Coca Cola Light" => "coca-cola-light.jpg",
            "Sisi" => "sisi.webp",
            "Tonic" => "tonic.jpg",
            "Schweppes Bitter Lemon" => "bitter-lemon.jpg",
            "Hertog Jan" => "hertog-jan.webp",
            "Duvel" => "duvel.webp",
            "Kriek" => "kriek.png",
            "Leffe Triple" => "leffe.webp",
            "House white wine" => "white-wine-bottle.webp",
            "House red wine" => "red-wine-bottle.webp",
            "Champagne" => "champagne.jpg",
            "Young Jenever" => "young-jenever.webp",
            "Whiskey" => "whiskey.webp",
            "Rum" => "rum.jpg",
            "Vieux" => "vieux.jpg",
            "Berenburg" => "berenburg.jpg",
            "Coffee" => "coffee.jpg",
            "Cappuchino" => "cappuccino.webp",
            "Espresso" => "espresso.jpg",
            "Tea" => "tea.jpg",
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
