using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class FinancialOverviewController : Controller
    {
        private readonly IFinancialService _financialService;

        public FinancialOverviewController(IFinancialService financialService)
        {
            _financialService = financialService;
        }

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            DateTime start = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = endDate ?? DateTime.Now;

            var model = _financialService.GetFinancialOverview(start, end);

            return View(model);
        }
    }
}
