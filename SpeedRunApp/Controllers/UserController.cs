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

namespace SpeedRunApp.MVC.Controllers
{
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
        public JsonResult UserSettings(UserSettingsViewModel userSettingsVM)
        {
            var success = false;

            try
            {
                var currUserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userService.SaveUserSettings(userSettingsVM, currUserID);

                if (userSettingsVM.UserID == currUserID) {
                    UpdateUserIdentity(currUserID);
                }

                success = true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Login");
                success = false;
            }

            return Json(new { success = success });
        }
                
        [HttpPost]
        public JsonResult UpdateIsDarkTheme(bool isDarkTheme)
        {
            var success = false;

            try
            {
                var currUserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userService.UpdateIsDarkTheme(currUserID, isDarkTheme);

                UpdateUserIdentity(currUserID);

                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateIsDarkTheme");
                success = false;
            }

            return Json(new { success = success });
        }

        private async void UpdateUserIdentity(int currUserID) {
            var userVW = _userService.GetUserViews(i => i.UserID == currUserID).FirstOrDefault();
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            
            if (identity != null) {
                HttpContext.User.AddUpdateClaim("theme", userVW.IsDarkTheme ? "theme-dark" : "theme-light");                                       

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }
        }
    }
}




