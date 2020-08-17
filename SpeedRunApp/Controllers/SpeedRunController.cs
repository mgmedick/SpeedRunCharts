using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;

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

        [HttpGet]
        public JsonResult SearchGamesAndUsers(string term)
        {
            var results = _speedRunService.SearchGamesAndUsers(term);

            return Json(results);
        }
    }
}
