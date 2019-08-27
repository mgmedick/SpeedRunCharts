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
            SpeedRunListViewModel speedRunListVM = new SpeedRunListViewModel();
            RunsService service = new RunsService();
            IEnumerable<RunDTO> runs = service.GetLatestRuns();
            List<SpeedRunViewModel> runList = new List<SpeedRunViewModel>();

            foreach (var run in runs)
            {
                runList.Add(new SpeedRunViewModel(run));
            }

            speedRunListVM.SpeedRuns = runList;

            return View(speedRunListVM);
        }
    }
}
