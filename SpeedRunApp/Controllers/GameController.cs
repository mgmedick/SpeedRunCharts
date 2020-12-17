using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;
using System.Collections.Generic;

namespace SpeedRunApp.WebUI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameService _gameService = null;
        private readonly ISpeedRunService _speedRunsService = null;

        public GameController(IGameService gameService, ISpeedRunService speedRunsService)
        {
            _gameService = gameService;
            _speedRunsService = speedRunsService;
        }

        public ViewResult GameDetails(string gameID)
        {
            var gameVM = _gameService.GetGame(gameID);

            return View(gameVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGrid(string ID)
        {
            var grid = _gameService.GetSpeedRunGrid(ID);

            return Json(new { GridModel = grid.Item1, GridData = grid.Item2 });
        }

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }
    }
}




