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
            //IEnumerable<Run> runs = speedRunComClient.Runs.GetRuns(orderBy: RunsOrdering.DateSubmittedDescending);
            List<SpeedRunViewModel> runs = new List<SpeedRunViewModel>();
            runs.Add(new SpeedRunViewModel(speedRunComClient.Runs.GetRun("y8198j5z")));

            List<string> links = new List<string>();
            foreach (Uri videolink in runs.FirstOrDefault().SpeedRun.Videos.Links)
            {
                links.Add(videolink.ToString());
            }

            speedRunListVM.SpeedRuns = runs;

            return View(speedRunListVM);
        }
    }
}
