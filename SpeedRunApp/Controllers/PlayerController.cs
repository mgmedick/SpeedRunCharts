using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System;
using Serilog;
using SpeedRunApp.Model;
using System.Linq;

namespace SpeedRunApp.MVC.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService = null;
        private readonly ISpeedRunService _speedRunService = null;
        private readonly ILogger _logger = null;

        public PlayerController(IPlayerService playerService, ISpeedRunService speedRunService, ILogger logger)
        {
            _playerService = playerService;
            _speedRunService = speedRunService;
            _logger = logger;
        }

        public ViewResult PlayerDetails(string ID, string speedRunCode)
        {
            var playerDetailsVM = _playerService.GetPlayerDetails(ID, speedRunCode);
            
            return View(playerDetailsVM);
        }

        [HttpGet]
        public JsonResult GetPlayerSpeedRunTabsAndData(int playerID)
        {
            PlayerDetailsTabViewModel tabVM = null;
            try
            {
                tabVM = _playerService.GetPlayerSpeedRunTabsAndData(playerID);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetPlayerSpeedRunTabsAndData PlayerID: {@PlayerID}", playerID);
            }

            return Json(tabVM);
        }

         [HttpGet]
        public JsonResult GetPlayerSpeedRunChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int playerID)
        {
            var results = _speedRunService.GetPlayerSpeedRunChartData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs, playerID);

            return Json(results);
        }          

        [HttpGet]
        public JsonResult SearchPlayers(string term)
        {
            var results = new List<SearchResult>();

            var players = _playerService.SearchPlayers(term).ToList();;
            if (players.Any()) {
                var playersGroup = new SearchResult { Value = "0", Label = "Players", SubItems = players };
                results.Add(playersGroup);
            }

            return Json(results);
        }          
    }
}




