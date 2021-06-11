using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
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

        public ViewResult GameDetails(int gameID)
        {
            var gameVM = _gameService.GetGame(gameID);

            return View(gameVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGrid(int ID)
        {
            var gridVM = _gameService.GetSpeedRunGrid(ID);

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gameService.SearchGames(term);

            return Json(results);
        }
    }
}




