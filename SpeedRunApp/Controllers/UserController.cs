﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using System.Linq;

namespace SpeedRunApp.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService = null;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ViewResult UserDetails(string userID)
        {
            var userVM = _userService.GetUser(userID);

            return View(userVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGrid(string ID)
        {
            var grid = _userService.GetSpeedRunGrid(ID);

            return Json(new { GridModel = grid.Item1, GridData = grid.Item2 });
        }

        [HttpGet]
        public JsonResult SearchUsers(string term)
        {
            var results = _userService.SearchUsers(term);

            return Json(results);
        }
    }
}




