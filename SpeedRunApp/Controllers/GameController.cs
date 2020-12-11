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
        private readonly IGamesService _gamesService = null;
        private readonly ISpeedRunsService _speedRunsService = null;

        public GameController(IGamesService gamesService, ISpeedRunsService speedRunsService)
        {
            _gamesService = gamesService;
            _speedRunsService = speedRunsService;
        }

        public ViewResult GameDetails(string gameID)
        {
            var gameVM = _gamesService.GetGameDetails(gameID);

            return View(gameVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGrid(string ID)
        {
            var gridVM = _gamesService.GetSpeedRunGridModel(ID);
            var gridData = _speedRunsService.GetLeaderboards(gridVM.GridItems);

            return Json(new { GridModel = gridVM, GridData = gridData });
        }

        //[HttpGet]
        //public JsonResult GetSpeedRunGridData(string ID)
        //{
        //    var runVMs = _speedRunService.GetSpeedRunsByGameID(ID);

        //    return Json(runVMs);
        //}

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gamesService.SearchGames(term);

            return Json(results);
        }

        /*
        [HttpGet]
        public JsonResult GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, string variableValues)
        {
            //var moderators = HttpContext.Session.Get<IEnumerable<IDNamePair>>("Moderators");
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, variableValues);

            return Json(recordVMs);
        }
        */
    }
}




