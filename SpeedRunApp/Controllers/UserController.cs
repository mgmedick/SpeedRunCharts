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
            var userVM = _userService.GetUser(userID);

            return View(userVM);
        }

        //[HttpPost]
        //public ViewResult UserDetails(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        //{
        //    var userVM = _userService.GetUser(userID, true);
        //    userVM.SpeedRuns = userVM.SpeedRuns.Where(i => (!drpCategoryTypes.Any() || drpCategoryTypes.Contains(((int)i.Category.Type).ToString()))
        //                                                && (!drpGames.Any() | drpGames.Contains(i.GameID))
        //                                                && (!drpCategories.Any() || drpCategories.Contains(i.CategoryID))
        //                                                && (!drpLevels.Any() || drpLevels.Contains(i.LevelID)))
        //                                        .ToList();

        //    return View(userVM);
        //}

        //[HttpGet]
        //public PartialViewResult UserDetailsGrid(string userID, IEnumerable<SpeedRun> speedRuns)
        //{
        //    var userGridVM = new UserDetailsGridViewModel(userID, speedRuns); //_userService.GetUserGrid(userID);
        //    return PartialView("_UserDetailsGrid", userGridVM);
        //}

        [HttpPost]
        public PartialViewResult SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        {
            var controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            var userGridVM = _userService.SearchUserSpeedRunGrid(userID, drpCategoryTypes, drpGames, drpCategories, drpLevels, controllerName);

            return PartialView("_SpeedRunGrid", userGridVM);
        }

        [HttpGet]
        public JsonResult UserSpeedRunGrid_Read(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _userService.GetUserSpeedRuns(userID, gameID, categoryType, categoryID, levelID);

            return Json(recordVMs);
        }

    }
}




