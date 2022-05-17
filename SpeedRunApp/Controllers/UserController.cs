using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SpeedRunApp.Interfaces.Services;
using System.Linq;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService = null;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

        [HttpGet]
        public JsonResult SearchUsers(string term)
        {
            var results = _userService.SearchUsers(term);

            return Json(results);
        }
    }
}




