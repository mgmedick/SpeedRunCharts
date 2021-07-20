﻿using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class SpeedRunService : ISpeedRunService
    {
        private readonly IConfiguration _config = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunService(IConfiguration config, IGameService gamesService, IUserService userService, ISpeedRunRepository speedRunRepo)
        {
            _config = config;
            _gamesService = gamesService;
            _userService = userService;
            _speedRunRepo = speedRunRepo;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            var elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);

            var runListVM = new SpeedRunListViewModel(elementsPerPage);

            return runListVM;
        }

        public IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
            IEnumerable<SpeedRunSummaryViewModel> runVMs = runs.Select(i => new SpeedRunSummaryViewModel(i));

            return runVMs;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(int gameID, int? speedRunID)
        {
            var gameVM = _gamesService.GetGame(gameID);
            var statusTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)RunStatusType.New, Name = RunStatusType.New .ToString() },
                                                       new IDNamePair() { ID = (int)RunStatusType.Rejected, Name = RunStatusType.Rejected .ToString() },
                                                       new IDNamePair() { ID = (int)RunStatusType.Verified, Name = RunStatusType.Verified .ToString() } };
            
            SpeedRunViewModel runVM = null;
            if (speedRunID.HasValue)
            {
                var run = _speedRunRepo.GetSpeedRunViews(i => i.ID == speedRunID.Value).FirstOrDefault();
                runVM = new SpeedRunViewModel(run);
            }

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameVM.CategoryTypes, gameVM.Categories, gameVM.Levels, gameVM.Platforms, gameVM.Variables, runVM);

            return editSpeedRunVM;
        }

        public SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID)
        {
            var run = _speedRunRepo.GetSpeedRunSummaryViews(i => i.ID == speedRunID).FirstOrDefault();
            var runVM = new SpeedRunSummaryViewModel(run);

            return runVM;
        }
    }
}
