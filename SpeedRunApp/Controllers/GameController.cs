using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeedRunApp.Model;
using SpeedRunApp.Service;
using SpeedRunCommon;

namespace SpeedRunApp.WebUI.Controllers
{
    public class GameController : Controller
    {
        public ViewResult GameDetails(string gameID)
        {
            GamesService service = new GamesService();
            var game = service.GetGame(gameID);

            return View(new GameDetailsViewModel(game));
        }

        [HttpGet]
        public JsonResult GetLeaderboardRecords(string gameID, string categoryID)
        {
            LeaderboardService service = new LeaderboardService();
            var runs = service.GetLeaderboardRecordsForCategory(gameID, categoryID);
            var recordVMs = runs.Select(i => new RecordViewModel(i));

            return Json(recordVMs);
        }
    }
}
