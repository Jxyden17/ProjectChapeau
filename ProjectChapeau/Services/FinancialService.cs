using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Services
{
    public class FinancialService : IFinancialService
    {
        private readonly IOrderRepository _orderRepository;

        public FinancialService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public FinancialOverview GetFinancialOverview(DateTime startDate, DateTime endDate)
        {
            var orders = _orderRepository.GetOrderByPeriod(startDate, endDate);

            return new FinancialOverview
            {
                StartDate = startDate,
                EndDate = endDate,
                /*TotalDrinkSales = orders.Where(o => o.Category == "Drink Menu").Sum(o => o.SalesAmount),
                TotalLunchSales = orders.Where(o => o.Category == "Lunch Menu").Sum(o => o.SalesAmount),
                TotalDinnerSales = orders.Where(o => o.Category == "Dinner Menu").Sum(o => o.SalesAmount),
                TotalDrinkIncome = orders.Where(o => o.Category == "Drink Menu").Sum(o => o.IncomeAmount),
                TotalLunchIncome = orders.Where(o => o.Category == "Lunch Menu").Sum(o => o.IncomeAmount),
                TotalDinnerIncome = orders.Where(o => o.Category == "Dinner Menu").Sum(o => o.IncomeAmount),
                TotalTipAmount = orders.Sum(o => o.TipAmount)*/
                TotalLunchSales = 30000,
                TotalDinnerSales = 30000,
                TotalDrinkSales = 30000,
                TotalLunchIncome = 30000,
                TotalDinnerIncome = 30000,
                TotalDrinkIncome = 30000,
                TotalTipAmount = 30000,
            };
        }
    }
}
