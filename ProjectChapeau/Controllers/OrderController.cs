using Microsoft.AspNetCore.Mvc;

namespace ProjectChapeau.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
