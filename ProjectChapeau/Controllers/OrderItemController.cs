﻿using Microsoft.AspNetCore.Mvc;

namespace ProjectChapeau.Controllers
{
    public class OrderItem : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}