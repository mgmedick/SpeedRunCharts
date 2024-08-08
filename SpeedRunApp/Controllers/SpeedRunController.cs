using System;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using System.Security.Claims;
using Serilog;

namespace SpeedRunApp.MVC.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IGameService _gamesService = null;
        private readonly IPlayerService _playerService = null;
        private readonly IUserService _userService = null;
        private readonly ILogger _logger = null;

        public SpeedRunController(ISpeedRunService speedRunService, IGameService gamesService, IPlayerService playerService, IUserService userService, ILogger logger)
        {
            _speedRunService = speedRunService;
            _gamesService = gamesService;
            _playerService = playerService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult GetSpeedRunListCategories()
        {
            var currUserID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var runListCategories = _speedRunService.GetSpeedRunListCategories(currUserID);

            return Json(runListCategories);
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var results = _speedRunService.GetLatestSpeedRuns(category, topAmount, orderValueOffset, categoryTypeID);

            return Json(results);
        }
        
        [HttpGet]
        public JsonResult GetSpeedRunSummary(int speedRunID)
        {
            var results = _speedRunService.GetSpeedRunSummary(speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetLeaderboardGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData)
        {
            var results = _speedRunService.GetLeaderboardGridData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs, showAllData);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetLeaderboardChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs)
        {
            var results = _speedRunService.GetLeaderboardGridData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs, true);

            return Json(results);
        }        

        [HttpGet]
        public JsonResult GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID)
        {
            var results = _speedRunService.GetWorldRecordGridData(gameID, categoryTypeID, categoryID, levelID);

            return Json(results);
        }
        
        [HttpGet]
        public JsonResult GetGameSummaryChartData(int gameID, int categoryTypeID)
        {
            var results = _speedRunService.GetGameSummaryChartData(gameID, categoryTypeID);

            return Json(results);
        }    

        [HttpGet]
        public JsonResult GetPlayerSpeedRunChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int playerID)
        {
            var results = _speedRunService.GetPlayerSpeedRunChartData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs, playerID);

            return Json(results);
        }                              
    }
}
