using Microsoft.AspNetCore.Mvc;
using MvcWhatsUp.Models.Extensions;
using ProjectChapeau.Models;
using ProjectChapeau.Models.ViewModel;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.ViewComponents
{
    public class CurrentOrderButtonViewComponent : ViewComponent
    {
        private const string orderSessionKey = "OrderSession";
        public IViewComponentResult Invoke(string buttonText)
        {
            var order = HttpContext.Session.GetObject<Order>(orderSessionKey);
            CurrentOrderButtonViewModel viewModel = new();
            if (order != null)
            {
                viewModel.ItemAmount = order.OrderLines.Count();
                viewModel.TotalPrice = order.TotalAmount;
            }
            viewModel.ButtonText = buttonText;

            return View(viewModel);
        }
    }
}
