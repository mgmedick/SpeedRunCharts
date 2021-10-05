using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Collections.Generic;

namespace SpeedRunApp.MVC.Controllers
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
        public JsonResult GetWorldRecordGridTabs(int gameID)
        {
            var gridVM = _gameService.GetWorldRecordGridTabs(gameID);

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridTabs(int ID, bool isGame)
        {
            var gridVM = isGame ? _gameService.GetSpeedRunGridTabs(ID) : _gameService.GetSpeedRunGridTabsForUser(ID);

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




