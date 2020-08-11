using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Service;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;
using Microsoft.Extensions.Configuration;

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
            HttpContext.Session.Set("Moderators", gameVM.Moderators);

            return View(gameVM);
        }

        [HttpGet]
        public JsonResult GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var moderators = HttpContext.Session.Get<IEnumerable<IDNamePair>>("Moderators");
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, moderators);

            return Json(recordVMs);
        }
    }
}




