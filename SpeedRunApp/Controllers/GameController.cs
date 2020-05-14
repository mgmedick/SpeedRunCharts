﻿using System;
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
        public JsonResult GetLeaderboardRecords(string gameID, CategoryType categoryType, string categoryID, string levelIDs)
        {
            LeaderboardService leaderboardService = new LeaderboardService();
            List<SpeedRunRecordDTO> records = new List<SpeedRunRecordDTO>();
            var levels = !string.IsNullOrWhiteSpace(levelIDs) ? levelIDs.Split(',').ToList() : new List<string>();

            switch (categoryType)
            {
                case CategoryType.PerGame:
                    records.AddRange(leaderboardService.GetLeaderboardRecordsForCategory(gameID, categoryID));
                    break;
                case CategoryType.PerLevel:
                    foreach (var levelID in levels)
                    {
                        records.AddRange(leaderboardService.GetLeaderboardRecordsForCategory(gameID, categoryID, levelID));
                    }
                    break;
            }

            var recordVMs = records.Select(i => new SpeedRunRecordViewModel(i));

            return Json(recordVMs);
        }

        [HttpGet]
        public JsonResult GetGameDetailsCharts()
        {
            List<string> charts = new List<string>() { "SpeedRunSummaryByMonth", "SpeedRunsReported", "SpeedRunsByUser" };

            return Json(charts.Select((v, i) => new { name = v, index = i }));
        }

        public JsonResult GetLeaderboardChartData(string gameID, string categoryID, DateTime startDate, DateTime endDate)
        {
            LeaderboardService leaderboardService = new LeaderboardService();
            List<SpeedRunRecordDTO> records = new List<SpeedRunRecordDTO>();

            records.AddRange(leaderboardService.GetLeaderboardRecordsForCategory(gameID, categoryID));

            var recordVMs = records.Where(i=>i.DateSubmitted >= startDate && i.DateSubmitted <= endDate).Select(i => new SpeedRunRecordViewModel(i));
            var timePeriods = DateTimeHelper.DateDiff("month", startDate, endDate).Select(i => i.ToString("MM/yyyy"));

            return Json(new { Data = recordVMs, TimePeriods = timePeriods });
        }
    }
}




