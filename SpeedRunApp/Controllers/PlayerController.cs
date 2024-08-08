using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
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

        public ViewResult PlayerDetails(string ID, string speedRunID)
        {
            var playerDetailsVM = _playerService.GetPlayerDetails(ID, speedRunID);
            
            return View(playerDetailsVM);
        }

        [HttpGet]
        public JsonResult SearchPlayers(string term)
        {
            var results = _playerService.SearchPlayers(term);

            return Json(results);
        }
    }
}




