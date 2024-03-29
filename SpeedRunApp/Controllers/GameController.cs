﻿using Microsoft.AspNetCore.Mvc;
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
        
        public ViewResult GameDetails(string ID, string speedRunID)
        {
            var gameDetailsVM = _gameService.GetGameDetails(ID, speedRunID);

            return View(gameDetailsVM);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null)
        {
            var results = _gameService.GetEditSpeedRun(gameID, speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetLeaderboardTabs(int gameID, int? speedRunID)
        {
            LeaderboardTabViewModel gridTabVM = null;
            try
            {
                gridTabVM = _gameService.GetLeaderboardTabs(gameID, speedRunID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetLeaderboardTabs GameID: {@GameID}, SpeedRunID: {@SpeedRunID}", gameID, speedRunID);
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
        public JsonResult GetUserSpeedRunTabsAndData(int userID, int? speedRunID)
        {
            UserSpeedRunTabViewModel tabVM = null;
            try
            {
                tabVM = _gameService.GetUserSpeedRunTabsAndData(userID, speedRunID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetUserSpeedRunTabsAndData UserID: {@UserID}, SpeedRunID: {@SpeedRunID}", userID, speedRunID);
            }

            return Json(tabVM);
        }

        [HttpGet]
        public JsonResult GetUserChartTabsAndData(int userID)
        {
            UserChartTabViewModel tabVM = null;
            try
            {
                tabVM = _gameService.GetUserChartTabsAndData(userID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetUserChartTabsAndData UserID: {@UserID}", userID);
            }

            return Json(tabVM);
        }        
        
       [HttpPost]
        public JsonResult SetGameIsChanged(int gameID)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                errorMessages = _gameService.SetGameIsChanged(gameID);
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SetGameIsChanged GameID: {@GameID}", gameID);
                success = false;
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }                          
    }
}




