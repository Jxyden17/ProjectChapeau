using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemController(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public IActionResult Index()
        {
            List<MenuItem> menuItem = _menuItemRepository.GetAllMenuItems();
            return View(menuItem);
        }
    }
}
