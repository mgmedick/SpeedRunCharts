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
            SpeedRunListViewModel speedRunListVM = GetSpeedRunList();
            return View(speedRunListVM);
        }

        public PartialViewResult SpeedRunListMore(int elementsOffset)
        {
            SpeedRunListViewModel speedRunListVM = GetSpeedRunList(elementsOffset);
            return PartialView("SpeedRunListMore", speedRunListVM);
        }

        private SpeedRunListViewModel GetSpeedRunList(int? elementsOffset = null)
        {
            SpeedRunListViewModel speedRunListVM = new SpeedRunListViewModel();
            RunsService service = new RunsService();
            IEnumerable<RunDTO> runs = service.GetLatestRuns(elementsOffset);
            List<SpeedRunViewModel> runList = new List<SpeedRunViewModel>();

            foreach (var run in runs)
            {
                runList.Add(new SpeedRunViewModel(run));
            }

            speedRunListVM.SpeedRuns = runList;

            return speedRunListVM;
        }
    }
}
