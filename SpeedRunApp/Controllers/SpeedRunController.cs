using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Service;
using SpeedRunApp.Interfaces.Services;

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
            var runListVM = _speedRunService.GetLatestSpeedRuns();
            return View(runListVM);
        }

        public PartialViewResult SpeedRunSummary(string speedRunID)
        {
            var runVM = _speedRunService.GetSpeedRun(speedRunID);
            return PartialView("_SpeedRunSummary", runVM);
        }

        public PartialViewResult SpeedRunListMore(int elementsOffset)
        {
            var runListVM = _speedRunService.GetLatestSpeedRuns(elementsOffset);
            return PartialView("_SpeedRunListMore", runListVM);
        }

        [HttpGet]
        public JsonResult SearchGamesAndUsers(string term)
        {
            var results = _speedRunService.SearchGamesAndUsers(term);

            return Json(results);
        }
    }
}
