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

        public PartialViewResult SpeedRunSummary(string speedRunID)
        {
            SpeedRunsService service = new SpeedRunsService();
            var speedRun = service.GetSpeedRun(speedRunID);
            return PartialView("_SpeedRunSummary", new SpeedRunViewModel(speedRun));
        }

        [HttpGet]
        public JsonResult GetLeaderboardRecords(string gameID, string categoryID, CategoryType categoryType)
        {
            GamesService gameService = new GamesService();
            LeaderboardService leaderboardService = new LeaderboardService();
            List<SpeedRunRecordDTO> records = new List<SpeedRunRecordDTO>();

            switch (categoryType)
            {
                case CategoryType.PerGame:
                    records.AddRange(leaderboardService.GetLeaderboardRecordsForCategory(gameID, categoryID));
                    break;
                case CategoryType.PerLevel:
                    var game = gameService.GetGame(gameID);
                    foreach (var level in game.Levels)
                    {
                        records.AddRange(leaderboardService.GetLeaderboardRecordsForCategory(gameID, categoryID, level.ID));
                    }
                    break;
            }

            var recordVMs = records.Select(i => new SpeedRunRecordViewModel(i));

            return Json(recordVMs);
        }
    }
}
