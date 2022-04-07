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

        public ViewResult GameDetails(string ID, int? speedRunID)
        {
            var gameDetailsVM = _gameService.GetGameDetails(ID, speedRunID);

            return View(gameDetailsVM);
        }

        //public ViewResult GameDetails(int gameID)
        //{
        //    var gameVM = _gameService.GetGame(gameID);

        //    return View(gameVM);
        //}

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
            var gridVM = _gameService.GetSpeedRunGridTabsForUser(userID, speedRunID);

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridTabs(int gameID, int? speedRunID)
        {
            var gridVM = _gameService.GetSpeedRunGridTabs(gameID, speedRunID);

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




