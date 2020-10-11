using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunsService _speedRunService = null;

        public SpeedRunController(ISpeedRunsService speedRunService)
        {
            _speedRunService = speedRunService;
        }

        public ViewResult SpeedRunList()
        {
            var runListVM = _speedRunService.GetSpeedRunList();
            return View(runListVM);
        }

        public PartialViewResult SpeedRunSummaryList(SpeedRunListCategory category, int elementsPerPage, int elementsOffset)
        {
            var runVMs = _speedRunService.GetLatestSpeedRuns(category, elementsPerPage, elementsOffset);
            return PartialView("_SpeedRunSummaryList", runVMs);
        }

        public PartialViewResult SpeedRunSummary(string speedRunID)
        {
            var runVM = _speedRunService.GetSpeedRun(speedRunID);
            return PartialView("_SpeedRunSummary", runVM);
        }

        public ViewResult SpeedRunDetails(string speedRunID)
        {
            var runVM = _speedRunService.GetSpeedRun(speedRunID);

            return View(runVM);
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
        public JsonResult GetEditSpeedRun(string speedRunID, bool isReadOnly)
        {
            var results = _speedRunService.GetEditSpeedRun(speedRunID, isReadOnly);

            return Json(results);
        }
    }
}
