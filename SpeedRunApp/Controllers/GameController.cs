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
        
        public ViewResult GameDetails(string ID, string speedRunID)
        {
            var gameDetailsVM = _gameService.GetGameDetails(ID, speedRunID);

            return View(gameDetailsVM);
        }
        
        [HttpGet]
        public JsonResult GetWorldRecordGridTabsForUser(int userID)
        {
            var gridVM = _gameService.GetSpeedRunGridTabsForUser(userID);

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetWorldRecordGridTabs(int gameID)
        {
            var gridVM = _gameService.GetWorldRecordGridTabs(gameID);

           return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridTabsForUser(int userID, int? speedRunID)
        {
            SpeedRunGridTabViewModel gridVM = null;
            try
            {
                gridVM = _gameService.GetSpeedRunGridTabsForUser(userID, speedRunID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetSpeedRunGridTabsForUser UserID: {@UserID}, SpeedRunID: {@SpeedRunID}", userID, speedRunID);
            }

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridTabs(int gameID, int? speedRunID)
        {
            SpeedRunGridTabViewModel gridVM = null;
            try
            {
                gridVM = _gameService.GetSpeedRunGridTabs(gameID, speedRunID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetSpeedRunGridTabs GameID: {@GameID}, SpeedRunID: {@SpeedRunID}", gameID, speedRunID);
            }

            return Json(gridVM);
        }

       [HttpPost]
        public JsonResult SetGameIsChanged(int gameID)
        {
            var success = false;

            try
            {
                _gameService.SetGameIsChanged(gameID);
                success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SetGameIsChanged");
                success = false;
            }

            return Json(new { success = success });
        }        

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }                     
    }
}




