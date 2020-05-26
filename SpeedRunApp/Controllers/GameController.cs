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
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.WebUI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGamesService _gamesService = null;
        private readonly ISpeedRunsService _speedRunService = null;
        private readonly ILeaderboardService _leaderboardService = null;

        public GameController(IGamesService gamesService, ISpeedRunsService speedRunService, ILeaderboardService leaderboardService)
        {
            _gamesService = gamesService;
            _speedRunService = speedRunService;
            _leaderboardService = leaderboardService;
        }

        public ViewResult GameDetails(string gameID)
        {
            var game = _gamesService.GetGame(gameID);

            return View(new GameDetailsViewModel(game));
        }

        public PartialViewResult SpeedRunSummary(string speedRunID)
        {
            var speedRun = _speedRunService.GetSpeedRun(speedRunID);
            return PartialView("_SpeedRunSummary", new SpeedRunViewModel(speedRun));
        }

        [HttpGet]
        public JsonResult GameDetails_Read(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _leaderboardService.GetLeaderboardRecords(gameID, categoryType, categoryID, levelID);

            return Json(recordVMs);
        }

        [HttpGet]
        public JsonResult GetGameDetailsCharts()
        {
            List<string> charts = new List<string>() { "SpeedRunSummaryByMonth", "SpeedRunsReported", "SpeedRunsByUser" };

            return Json(charts.Select((v, i) => new { name = v, index = i }));
        }

        public JsonResult GetSpeedRunsByUserChartData(string gameID, CategoryType categoryType, string categoryID, string levelID, int topAmount)
        {
            var recordVMs = _leaderboardService.GetLeaderboardRecords(gameID, categoryType, categoryID, levelID);
            recordVMs = recordVMs.OrderBy(i => i.PrimaryRunTimeSeconds).Take(topAmount);

            return Json(new { Data = recordVMs });
        }

        public JsonResult GetSpeedRunsReportedChartData(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _leaderboardService.GetLeaderboardRecords(gameID, categoryType, categoryID, levelID);

            return Json(new { Data = recordVMs });
        }

        public JsonResult GetSpeedRunSummaryByMonthChartData(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _leaderboardService.GetLeaderboardRecords(gameID, categoryType, categoryID, levelID);

            //DateTime startDate = recordVMs.Where(i=>i.DateSubmitted.HasValue).Select(i => i.DateSubmitted.Value).OrderBy(i => i).FirstOrDefault();
            DateTime endDate = recordVMs.Where(i => i.DateSubmitted.HasValue).Select(i => i.DateSubmitted.Value).OrderBy(i => i).LastOrDefault();
            DateTime startDate = (endDate != DateTime.MinValue) ? endDate.AddMonths(-6) : DateTime.MinValue;

            var timePeriods = DateTimeHelper.DateDiff("month", startDate, endDate).Select(i => i.ToString("MM/yyyy"));

            return Json(new { Data = recordVMs, TimePeriods = timePeriods });
        }
    }
}




