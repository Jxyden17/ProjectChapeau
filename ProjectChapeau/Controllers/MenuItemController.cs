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

        public IActionResult Index(int? menuId, int? categoryId)
        {
            var menuItems = _menuItemRepository.GetFilteredMenuItems(menuId, categoryId);
            return View(menuItems);


        }

        [HttpGet]
        public ActionResult AddMenuItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMenuItem(MenuItem menuItem)
        {
            try
            {
                _menuItemRepository.AddMenuItem(menuItem);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(menuItem);
            }
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(MenuItem menuItem)
        {
            try
            {
                _menuItemRepository.AddMenuItem(menuItem);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(menuItem);
            }
        }

        public IActionResult Deactivate(int id)
        {
            _menuItemRepository.DeactivateMenuItem(id);
            return RedirectToAction("Index");
        }


    }

}
