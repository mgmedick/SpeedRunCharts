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

        public ViewResult GameDetails(string ID)
        {
            var gameVM = _gameService.GetGame(ID);

            return View(gameVM);
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
        public JsonResult GetSpeedRunGridTabsForUser(int userID)
        {
            var gridVM = _gameService.GetSpeedRunGridTabsForUser(userID);

            return Json(gridVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridTabs(int gameID)
        {
            var gridVM = _gameService.GetSpeedRunGridTabs(gameID);

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




