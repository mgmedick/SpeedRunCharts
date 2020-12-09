using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        /*
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
        */
    }
}




