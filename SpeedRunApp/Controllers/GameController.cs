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
        private readonly ISpeedRunsService _speedRunService = null;

        public GameController(IGamesService gamesService, ISpeedRunsService speedRunService)
        {
            _gamesService = gamesService;
            _speedRunService = speedRunService;
        }

        public ViewResult GameDetails(string gameID)
        {
            var gameVM = _gamesService.GetGameDetails(gameID);

            return View(gameVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridModel(string ID)
        {
            var gridModelVM = _gamesService.GetSpeedRunGridModel(ID);

            return Json(gridModelVM);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridData(string ID)
        {
            var runVMs = _speedRunService.GetSpeedRunsByGameID(ID);

            return Json(runVMs);
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




