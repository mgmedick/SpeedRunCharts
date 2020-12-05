using System;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunsService _speedRunService = null;
        private readonly ISpeedRunsService1 _speedRunService1 = null;

        public SpeedRunController(ISpeedRunsService speedRunService, ISpeedRunsService1 speedRunService1)
        {
            _speedRunService = speedRunService;
            _speedRunService1 = speedRunService1;
        }

        public ViewResult SpeedRunList()
        {
            var runListVM = _speedRunService1.GetSpeedRunList();

            return View(runListVM);
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var results = _speedRunService1.GetLatestSpeedRuns(category, topAmount, orderValueOffset);

            return Json(results);
        }

        [HttpGet]
        public JsonResult SearchGamesAndUsers(string term)
        {
            var results = _speedRunService.SearchGamesAndUsers(term);

            return Json(results);
        }

        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _speedRunService.SearchGames(term);

            return Json(results);
        }

        [HttpGet]
        public JsonResult SearchUsers(string term)
        {
            var results = _speedRunService.SearchUsers(term);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(string speedRunID, string gameID, bool isReadOnly)
        {
            var results = _speedRunService1.GetEditSpeedRun(speedRunID, gameID, isReadOnly);

            return Json(results);
        }
    }
}
