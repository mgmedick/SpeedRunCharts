using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SpeedRunApp.Service
{
    public class SpeedRunService : ISpeedRunService
    {
        private readonly IConfiguration _config = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IUserRepository _userRepo = null;
        private readonly ISettingRepository _settingRepo = null;

        public SpeedRunService(IConfiguration config, ISpeedRunRepository speedRunRepo, IUserRepository userRepo, ISettingRepository settingRepo)
        {
            _config = config;
            _speedRunRepo = speedRunRepo;
            _userRepo = userRepo;
            _settingRepo = settingRepo;
        }

        public IEnumerable<SummaryList> GetSummaryLists(int currUserID)
        {
            var allSpeedRunSummaryLists = _speedRunRepo.GetSummaryLists().ToList();
            var speedRunSummaryLists = allSpeedRunSummaryLists.Where(i => i.IsDefault).OrderBy(i => i.DefaultSortOrder).ToList();

            if (currUserID > 0)
            {
                var userSummaryLists = _userRepo.GetUserSummaryLists(i => i.UserID == currUserID);
                if(userSummaryLists.Any())
                {
                    speedRunSummaryLists = (from c in speedRunSummaryLists
                                join uc in userSummaryLists
                                on c.ID equals uc.SummaryListID
                                orderby uc.ID
                                select c).ToList();
                }
            }
            
            return speedRunSummaryLists;
        }

        public IEnumerable<SpeedRunSummaryViewModel> GetSummaryListResults(int summaryListID, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var runs = _speedRunRepo.GetSummaryListResults(summaryListID, topAmount, orderValueOffset, categoryTypeID);
            IEnumerable<SpeedRunSummaryViewModel> runVMs = runs.Select(i => new SpeedRunSummaryViewModel(i));

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData)
        {
            var runs = new List<SpeedRunGridView>();
            if (showAllData) {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
            } else {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs && i.Rank.HasValue).OrderBy(i => i.PrimaryTime).ToList();
            }

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetLeaderboardChartData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }         

        public IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID)
        {                                     
            var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && (!categoryID.HasValue || i.CategoryID == categoryID) && (!levelID.HasValue || i.LevelID == levelID) && i.Rank == 1)
                                    .OrderBy(i => i.CategoryID)
                                    .ThenBy(i => i.LevelID)
                                    .ThenBy(i => i.SubCategoryVariableValueIDs)
                                    .ToList();
                                                                                                            
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID && g.LevelID == i.LevelID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetGameSummaryChartData(int gameID, int categoryTypeID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && ((categoryTypeID == 0 && !i.LevelID.HasValue) || (categoryTypeID == 1 && i.LevelID.HasValue)))
                                    .OrderBy(i => i.CategoryID)
                                    .ThenBy(i => i.LevelID)
                                    .ThenBy(i => i.SubCategoryVariableValueIDs)
                                    .ToList();

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunGridData(int playerID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridPlayerViews(i => i.PlayerID == playerID).ToList();            
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var personalBests = runVMs.Where(i => i.Rank.HasValue)
                                      .OrderBy(i => i.Rank)
                                      .GroupBy(g => new { g.GameID, g.CategoryID, g.LevelID, g.SubCategoryVariableValueIDs })
                                      .Select(i => i.First())
                                      .ToList();

            foreach(var personalBest in personalBests)
            {
                personalBest.IsPersonalBest = true;
            }

            runVMs = runVMs.OrderBy(i=>i.GameID)
                            .ThenBy(i=>i.CategoryID)
                            .ThenBy(i=>i.LevelID)
                            .ThenBy(i=>i.SubCategoryVariableValueIDs)
                            .ThenByDescending(i=>i.DateSubmitted ?? DateTime.MinValue)
                            .ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int playerID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridPlayerViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs && i.PlayerID == playerID).OrderByDescending(i => i.ID).ToList();     
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }     
    }
}
