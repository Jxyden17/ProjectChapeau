using Microsoft.AspNetCore.Mvc;
using ProjectChapeau.Models;
using ProjectChapeau.Repositories.Interfaces;
using ProjectChapeau.Services.Interfaces;

namespace ProjectChapeau.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public IActionResult Index(int? menuId, int? categoryId)
        {
            var menuItems = _menuItemService.GetFilteredMenuItems(menuId, categoryId);
            return View(menuItems);


        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(MenuItem menuItem)
        {
            try
            {
                _menuItemService.AddMenuItem(menuItem);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
				ViewData["ErrorMessage"] = ex.Message;
				return View(menuItem);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            MenuItem? menuItem = _menuItemService.GetMenuItemById(id);
            return View(menuItem);
        }

        [HttpPost]
        public ActionResult Edit(MenuItem menuItem)
        {
            try
            {
                _menuItemService.UpdateMenuItem(menuItem);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
				    ViewData["ErrorMessage"] = ex.Message;
				     return View(menuItem);
            }
        }

        public IActionResult Deactivate(int id)
        {
            _menuItemService.DeactivateMenuItem(id);
            return RedirectToAction("Index");
        }

        public IActionResult Activate(int id)
        {
            _menuItemService.ActivateMenuItem(id);
            return RedirectToAction("Index");
        }
    }

}
