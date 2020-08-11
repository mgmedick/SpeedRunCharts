using System;
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
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;

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

        [HttpGet]
        public ViewResult UserDetails(string userID)
        {
            var userVM = _userService.GetUserDetails(userID);

            return View(userVM);
        }

        [HttpGet]
        public JsonResult GetUserSpeedRuns(string userID, int elementsPerPage, int elementsOffset)
        {
            var runVMs = _userService.GetUserSpeedRuns(userID, elementsPerPage, elementsOffset);

            return Json(runVMs);
        }
    }
}




