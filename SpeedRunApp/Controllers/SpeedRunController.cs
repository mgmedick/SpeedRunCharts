using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.WebUI.Models;
using SpeedrunComSharp;

namespace SpeedRunApp.WebUI.Controllers
{
    public class SpeedRunController : Controller
    {
        public ViewResult SpeedRunList()
        {
            SpeedRunListViewModel speedRunListVM = new SpeedRunListViewModel();

            var speedRunComClient = new SpeedrunComClient();
            IEnumerable<Run> runs = speedRunComClient.Runs.GetRuns(orderBy: RunsOrdering.DateSubmittedDescending);
            speedRunListVM.SpeedRuns = runs;

            return View(speedRunListVM);
        }
    }
}
