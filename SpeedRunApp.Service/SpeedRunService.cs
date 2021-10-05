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
    public class SpeedRunService : ISpeedRunService
    {
        private readonly IConfiguration _config = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ICacheService _cacheService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunService(IConfiguration config, IGameService gamesService, IUserService userService, ICacheService cacheService, ISpeedRunRepository speedRunRepo)
        {
            _config = config;
            _gamesService = gamesService;
            _userService = userService;
            _cacheService = cacheService;
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

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, runVM.Players, gameVM.CategoryTypes, gameVM.Categories, gameVM.Levels, gameVM.Platforms, gameVM.Variables, runVM);

            return editSpeedRunVM;
        }

        public SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID)
        {
            var run = _speedRunRepo.GetSpeedRunSummaryViews(i => i.ID == speedRunID).FirstOrDefault();
            var runVM = new SpeedRunSummaryViewModel(run);

            return runVM;
        }

        public IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.Rank == 1).ToList();
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            runVMs = runVMs.Where(i => i.VariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.CategoryID == i.CategoryID).Select(h => h.VariableValueIDs?.Split(",").Count()).Max()).ToList();
            
            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetSpeedRunGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string variableValueIDs, int? userID)
        {
            var runs = new List<SpeedRunGridView>();
            if (userID.HasValue)
            {
                runs = _speedRunRepo.GetSpeedRunGridViewsByUserID(gameID, categoryTypeID, categoryID, levelID, variableValueIDs, userID.Value).ToList();
            }
            else
            {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.CategoryID == categoryID && i.LevelID == levelID && i.VariableValueIDs == variableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
            }

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }
    }
}
