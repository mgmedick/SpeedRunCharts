using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Model;
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
            var runListVM = GetSpeedRunListVM();
            return View(runListVM);
        }

        public PartialViewResult SpeedRunListMore(int elementsOffset)
        {
            var runListVM = GetSpeedRunListVM(elementsOffset);
            return PartialView("_SpeedRunListMore", runListVM);
        }

        private SpeedRunListViewModel GetSpeedRunListVM(int? elementsOffset = null)
        {
            var runs = _speedRunService.GetLatestSpeedRuns(elementsOffset);
            var runsVM = runs.Select(i => new SpeedRunViewModel(i));

            return new SpeedRunListViewModel() { SpeedRuns = runsVM };
        }
    }
}
