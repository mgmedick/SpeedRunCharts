using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Collections.Generic;
using System;
using Serilog;

namespace SpeedRunApp.MVC.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService = null;
        private readonly ISpeedRunService _speedRunService = null;
        private readonly ILogger _logger = null;

        public GameController(IGameService gameService, ISpeedRunService speedRunService, ILogger logger)
        {
            _gameService = gameService;
            _speedRunService = speedRunService;
            _logger = logger;
        }
        
        public ViewResult GameDetails(string ID, string speedRunCode)
        {
            var gameDetailsVM = _gameService.GetGameDetails(ID, speedRunCode);

            return View(gameDetailsVM);
        }

        /*
        [HttpGet]
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null)
        {
            var results = _gameService.GetEditSpeedRun(gameID, speedRunID);

            return Json(results);
        }
        */

        [HttpGet]
        public JsonResult GetLeaderboardTabs(int gameID, string speedRunCode)
        {
            GameDetailsTabViewModel gridTabVM = null;
            try
            {
                gridTabVM = _gameService.GetLeaderboardTabs(gameID, speedRunCode);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetLeaderboardTabs GameID: {@GameID}, SpeedRunCode: {@SpeedRunCode}", gameID, speedRunCode);
            }

            return Json(gridTabVM);
        }

        [HttpGet]
        public JsonResult GetWorldRecordTabs(int gameID)
        {
            var tabVM = _gameService.GetWorldRecordTabs(gameID);

           return Json(tabVM);
        }

        [HttpGet]
        public JsonResult GetGameChartTabs(int gameID)
        {
            var tabVM = _gameService.GetGameChartTabs(gameID);

           return Json(tabVM);
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
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }                          
    }
}




