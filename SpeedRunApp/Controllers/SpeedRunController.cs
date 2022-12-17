using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Xml;
using Microsoft.AspNetCore.Http.Features;

namespace SpeedRunApp.MVC.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly IUserAccountService _userAcctService = null;
        private readonly ILogger _logger = null;

        public SpeedRunController(ISpeedRunService speedRunService, IGameService gamesService, IUserService userService, IUserAccountService userAcctService, ILogger logger)
        {
            _speedRunService = speedRunService;
            _gamesService = gamesService;
            _userService = userService;
            _userAcctService = userAcctService;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult GetSpeedRunListCategories()
        {
            var currUserAccountID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var runListCategories = _speedRunService.GetSpeedRunListCategories(currUserAccountID);

            return Json(runListCategories);
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var results = _speedRunService.GetLatestSpeedRuns(category, topAmount, orderValueOffset, categoryTypeID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null)
        {
            var results = _speedRunService.GetEditSpeedRun(gameID, speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSpeedRunSummary(int speedRunID)
        {
            var results = _speedRunService.GetSpeedRunSummary(speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetGameWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID, string subCategoryVariableValueIDs)
        {
            var results = _speedRunService.GetGameWorldRecordGridData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetUserPersonalBestGridData(int gameID, int categoryID, int? levelID, int userID)
        {
            var results = _speedRunService.GetUserPersonalBestGridData(gameID, categoryID, levelID, userID);

            return Json(results);
        }        

        [HttpGet]
        public JsonResult GetGameSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData)
        {
            var results = _speedRunService.GetGameSpeedRunGridData(gameID, categoryID, levelID, subCategoryVariableValueIDs, showAllData);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetUserSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID)
        {
            var results = _speedRunService.GetUserSpeedRunGridData(gameID, categoryID, levelID, subCategoryVariableValueIDs, userID);

            return Json(results);
        }
    }
}
