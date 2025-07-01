namespace ProjectChapeau.Models.ViewModel
{
    public class CurrentOrderButtonViewModel
    {
        public string ButtonText { get; set; } = string.Empty;
        public int ItemAmount { get; set; } = 0;
        public decimal TotalPrice { get; set; } = decimal.Zero;

        public CurrentOrderButtonViewModel()
        {}
        public CurrentOrderButtonViewModel(string? buttonText, int itemAmount, decimal totalPrice)
        {
            ButtonText = buttonText;
            ItemAmount = itemAmount;
            TotalPrice = totalPrice;
        }
    }
}
