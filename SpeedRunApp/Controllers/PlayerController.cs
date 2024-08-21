using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System;
using Serilog;

namespace SpeedRunApp.MVC.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService = null;
        private readonly ILogger _logger = null;

        public PlayerController(IPlayerService playerService, ILogger logger)
        {
            _playerService = playerService;
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
        public JsonResult SearchPlayers(string term)
        {
            var results = _playerService.SearchPlayers(term);

            return Json(results);
        }
    }
}




