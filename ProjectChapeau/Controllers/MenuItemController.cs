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
            catch (Exception)
            {
                TempData["ErrorMessage"] = $"{menuItem.ItemName} could not be added";
                return View(menuItem);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                MenuItem menuItem = _menuItemService.GetMenuItemById(id);
                return View(menuItem);
            }
            catch (Exception)
            {
                return NotFound($"Menu item with ID {id} not found.");
            }
        }

        [HttpPost]
        public ActionResult Edit(MenuItem menuItem)
        {
            try
            {
                _menuItemService.UpdateMenuItem(menuItem);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = $"Menu item {menuItem.ItemName} could not be edited";
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
