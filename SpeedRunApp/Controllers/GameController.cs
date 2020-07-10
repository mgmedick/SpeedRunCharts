using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Service;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;

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
        public JsonResult GameSpeedRunGrid_Read(string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate = null, DateTime? endDate = null)
        {
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, startDate, endDate);

            return Json(recordVMs);
        }

        [HttpGet]
        public JsonResult GetGameDetailsCharts()
        {
            List<string> charts = new List<string>() { "SpeedRunSummaryByMonth", "SpeedRunsReported", "SpeedRunsByUser" };

            return Json(charts.Select((v, i) => new { name = v, index = i }));
        }

        [HttpPost]
        public PartialViewResult SearchGameSpeedRunGrid(string gameID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels)
        {
            var gameGridVM = _gamesService.SearchGameSpeedRunGrid(gameID, drpCategoryTypes, drpGames, drpCategories, drpLevels);

            return PartialView("_SpeedRunGrid", gameGridVM);
        }

        public JsonResult GetSpeedRunsByUserChartData(string gameID, CategoryType categoryType, string categoryID, string levelID, int topAmount)
        {
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, null, null);
            recordVMs = recordVMs.Take(topAmount);

            return Json(new { Data = recordVMs });
        }

        public JsonResult GetSpeedRunsReportedChartData(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, null, null);

            return Json(new { Data = recordVMs });
        }

        public JsonResult GetSpeedRunSummaryByMonthChartData(string gameID, CategoryType categoryType, string categoryID, string levelID)
        {
            var recordVMs = _gamesService.GetGameSpeedRunRecords(gameID, categoryType, categoryID, levelID, null, null);

            DateTime endDate = recordVMs.Where(i => i.DateSubmitted.HasValue).Select(i => i.DateSubmitted.Value).OrderBy(i => i).LastOrDefault();
            DateTime startDate = (endDate != DateTime.MinValue) ? endDate.AddMonths(-6) : DateTime.MinValue;

            var timePeriods = DateTimeHelper.DateDiff("month", startDate, endDate).Select(i => i.ToString("MM/yyyy"));

            return Json(new { Data = recordVMs, TimePeriods = timePeriods });
        }
    }
}




