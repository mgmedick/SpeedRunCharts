using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Model;
using SpeedRunApp.Service;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
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
            SpeedRunsService service = new SpeedRunsService();
            var runs = service.GetLatestSpeedRuns(elementsOffset);
            var runsVM = runs.Select(i => new SpeedRunViewModel(i));

            return new SpeedRunListViewModel() { SpeedRuns = runsVM };
        }
    }
}
