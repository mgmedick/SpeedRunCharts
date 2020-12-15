using Microsoft.Extensions.Configuration;
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
    public class SpeedRunsService : ISpeedRunsService
    {
        private readonly IConfiguration _config = null;
        private readonly IGamesService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunsService(IConfiguration config, IGamesService gamesService, IUserService userService, ISpeedRunRepository speedRunRepo)
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

        public IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
            IEnumerable<SpeedRunViewModel> runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(string gameID, string speedRunID, bool isReadOnly)
        {
            var gameVM = _gamesService.GetGame(gameID);
            var statusTypes = _speedRunRepo.RunStatusTypes();

            SpeedRunViewModel runVM = null;
            if (!string.IsNullOrWhiteSpace(speedRunID))
            {
                var run = _speedRunRepo.GetSpeedRunView(speedRunID);
                runVM = new SpeedRunViewModel(run);
            }

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameVM.CategoryTypes, gameVM.Categories, gameVM.Levels, gameVM.Platforms, gameVM.Variables, runVM, isReadOnly);

            return editSpeedRunVM;
        }

        public IEnumerable<SpeedRunViewModel> GetSpeedRunsByGameID(string gameID)
        {
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.StatusTypeID == (int)RunStatusType.Verified && i.Rank.HasValue).OrderBy(i => i.Rank);
            var runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }
    }
}
