using Microsoft.AspNetCore.Mvc;

namespace ProjectChapeau.Services.Interfaces
{
	public class IMenuItemService : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
