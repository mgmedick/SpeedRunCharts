using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using System.Collections.Generic;
using System;
using Serilog;

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

        public ViewResult UserDetails(string ID, string speedRunID)
        {
            var userDetailsVM = _userService.GetUserDetails(ID, speedRunID);
            
            return View(userDetailsVM);
        }

        //public ViewResult UserDetails(int userID)
        //{
        //    var userVM = _userService.GetUser(userID);

        //    return View(userVM);
        //}

       [HttpPost]
        public JsonResult SetUserIsChanged(int userID)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                errorMessages = _userService.SetUserIsChanged(userID);
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SetUserIsChanged UserID: {@UserID}", userID);
                success = false;
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public JsonResult SearchUsers(string term)
        {
            var results = _userService.SearchUsers(term);

            return Json(results);
        }
    }
}




