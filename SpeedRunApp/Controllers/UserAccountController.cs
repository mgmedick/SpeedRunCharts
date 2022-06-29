using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using System.Linq;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using Serilog;
using System.Security.Claims;

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
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SaveUserAccount");
                success = false;
            }

            return Json(new { success = success });
        }
    }
}




