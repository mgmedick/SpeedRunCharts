﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeedRunApp.Model;
using SpeedRunApp.Service;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService = null;
        private readonly IMemoryCache _cache = null;

        public UserController(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        public ViewResult UserDetails(string userID)
        {
            var userVM = _userService.GetUser(userID, true);

            return View(userVM);
        }

        [HttpGet]
        public JsonResult UserDetails_Read(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _userService.GetUserSpeedRuns(userID, gameID, categoryType, categoryID, levelID);

            return Json(recordVMs);
        }
    }
}




