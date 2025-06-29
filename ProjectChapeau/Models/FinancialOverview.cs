namespace ProjectChapeau.Models
{
    public class FinancialOverview
    {
 
        public decimal TotalDrinkSales { get; set; }
        public decimal TotalLunchSales { get; set; }
        public decimal TotalDinnerSales { get; set; }
        public decimal TotalDrinkIncome { get; set; }
        public decimal TotalLunchIncome { get; set; }
        public decimal TotalDinnerIncome { get; set; }
        public decimal TotalTipAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public FinancialOverview()
        {

        }

        public FinancialOverview(decimal totalDrinkSales, decimal totalLunchSales, decimal totalDinnerSales, decimal totalDrinkIncome, decimal totalLunchIncome, decimal totalDinnerIncome, decimal totalTipAmount)
        {
            TotalDrinkSales = totalDrinkSales;
            TotalLunchSales = totalLunchSales;
            TotalDinnerSales = totalDinnerSales;
            TotalDrinkIncome = totalDrinkIncome;
            TotalLunchIncome = totalLunchIncome;
            TotalDinnerIncome = totalDinnerIncome;
            TotalTipAmount = totalTipAmount;
        }

    }
}
