using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunCommon.Extensions;
using System.Collections.Generic;
using System;
using Serilog;
using System.Linq;

namespace SpeedRunApp.MVC.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly ILogger _logger = null;

        public SpeedRunController(ISpeedRunService speedRunService, ILogger logger)
        {
            _speedRunService = speedRunService;
            _logger = logger;
        }
        
        public ViewResult SpeedRunDetails(string ID)
        {
            var runDetailsVM = _speedRunService.GetSpeedRunDetails(ID);

            return View(runDetailsVM);
        }  

        public JsonResult GetSpeedRunDetails(int speedRunID)
        {
            var runDetailsVM = _speedRunService.GetSpeedRunDetails(speedRunID);

            return Json(runDetailsVM);
        }       
    }
}




