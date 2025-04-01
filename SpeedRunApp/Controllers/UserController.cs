using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using System.Linq;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using Serilog;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SpeedRunCommon.Extensions;
using SpeedRunApp.Model.Data;
using Microsoft.AspNetCore.Authorization;

namespace SpeedRunApp.MVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService = null;
        private readonly ILogger _logger = null;

        public UserController(IUserService userService, ILogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public ViewResult UserSettings()
        {
            var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userSettingsVM = _userService.GetUserSettings(userID);

            return View(userSettingsVM);   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveUserSummaryLists(List<int> summaryListIDs)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var userID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userService.SaveUserSummaryLists(userID, summaryListIDs);

                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SaveUserSummaryLists");
                success = false;
                errorMessages = new List<string>() { "Error saving user summary lists" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }
    }
}




