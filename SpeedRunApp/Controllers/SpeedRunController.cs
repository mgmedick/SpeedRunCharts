using System;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunsService _speedRunService = null;
        private readonly IGamesService _gamesService = null;
        private readonly IUserService _userService = null;

        public SpeedRunController(ISpeedRunsService speedRunService1, IGamesService gamesService, IUserService userService)
        {
            _speedRunService = speedRunService1;
            _gamesService = gamesService;
            _userService = userService;
        }

        public ViewResult SpeedRunList()
        {
            var runListVM = _speedRunService.GetSpeedRunList();

            return View(runListVM);
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var results = _speedRunService.GetLatestSpeedRuns(category, topAmount, orderValueOffset);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(string gameID, string speedRunID = null, bool isReadOnly = false)
        {
            var results = _speedRunService.GetEditSpeedRun(speedRunID, gameID, isReadOnly);

            return Json(results);
        }
    }
}
