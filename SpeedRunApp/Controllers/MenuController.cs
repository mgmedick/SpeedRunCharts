using System;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.MVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService = null;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public ViewResult About()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Search(string term)
        {
            var results = _menuService.Search(term);

            return Json(results);
        }
    }
}
