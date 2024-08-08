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
        private readonly ISpeedRunService _speedRunsService = null;
        private readonly ILogger _logger = null;

        public GameController(IGameService gameService, ISpeedRunService speedRunsService, ILogger logger)
        {
            _gameService = gameService;
            _speedRunsService = speedRunsService;
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
            LeaderboardTabViewModel gridTabVM = null;
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
        public JsonResult GetPlayerSpeedRunTabsAndData(int playerID)
        {
            PlayerSpeedRunTabViewModel tabVM = null;
            try
            {
                tabVM = _gameService.GetPlayerSpeedRunTabsAndData(playerID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetPlayerSpeedRunTabsAndData PlayerID: {@PlayerID}", playerID);
            }

            return Json(tabVM);
        }

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }                          
    }
}




