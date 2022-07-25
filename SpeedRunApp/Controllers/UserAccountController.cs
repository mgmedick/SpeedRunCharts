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

namespace SpeedRunApp.MVC.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly IUserAccountService _userAccountService = null;
        private readonly ILogger _logger = null;

        public UserAccountController(IUserAccountService userAccountService, ILogger logger)
        {
            _userAccountService = userAccountService;
            _logger = logger;
        }

        [HttpGet]
        public ViewResult UserAccountDetails()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserAccount(int userAccountID)
        {
            var userAcctVM = _userAccountService.GetUserAccount(userAccountID);

            return Json(userAcctVM);
        }

        [HttpPost]
        public JsonResult SaveUserAccount(UserAccountViewModel userAcctVM)
        {
            var success = false;

            try
            {
                var currUserAccountID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userAccountService.SaveUserAccount(userAcctVM, currUserAccountID);

                if (userAcctVM.UserAccountID == currUserAccountID) {
                    UpdateUserIdentity(currUserAccountID);
                }

                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SaveUserAccount");
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
                var currUserAccountID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _userAccountService.UpdateIsDarkTheme(currUserAccountID, isDarkTheme);

                UpdateUserIdentity(currUserAccountID);

                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateIsDarkTheme");
                success = false;
            }

            return Json(new { success = success });
        }

        private async void UpdateUserIdentity(int currUserAccountID) {
            var userAcctVW = _userAccountService.GetUserAccountViews(i => i.UserAccountID == currUserAccountID).FirstOrDefault();
            var identity = (ClaimsIdentity)HttpContext.User.Identity;
            
            if (identity != null) {
                HttpContext.User.AddUpdateClaim("theme", userAcctVW.IsDarkTheme ? "theme-dark" : "theme-light");                                       

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            }
        }
    }
}




