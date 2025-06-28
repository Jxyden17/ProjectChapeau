using ProjectChapeau.Models;

namespace ProjectChapeau.Services.Interfaces
{
    public interface IFinancialService
    {
        FinancialOverview GetFinancialOverview(DateTime startDate, DateTime endDate);
    }
}
