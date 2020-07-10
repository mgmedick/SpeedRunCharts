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
        public JsonResult UserSpeedRunGrid_Read(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate = null, DateTime? endDate = null)
        {
            var recordVMs = _userService.GetUserSpeedRuns(userID, gameID, categoryType, categoryID, levelID, startDate, endDate);

            return Json(recordVMs);
        }

        [HttpPost]
        public PartialViewResult SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        {
            var userGridVM = _userService.SearchUserSpeedRunGrid(userID, drpCategoryTypes, drpGames, drpCategories, drpLevels);

            return PartialView("_SpeedRunGrid", userGridVM);
        }
    }
}




