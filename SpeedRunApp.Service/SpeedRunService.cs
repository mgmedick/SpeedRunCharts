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
        private readonly IUserService _userService = null;
        private readonly ICacheService _cacheService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IUserAccountRepository _userAcctRepo = null;
        private readonly ISettingRepository _settingRepo = null;

        public SpeedRunService(IConfiguration config, IUserService userService, ICacheService cacheService, ISpeedRunRepository speedRunRepo, IUserAccountRepository userAcctRepo, ISettingRepository settingRepo)
        {
            _config = config;
            _userService = userService;
            _cacheService = cacheService;
            _speedRunRepo = speedRunRepo;
            _userAcctRepo = userAcctRepo;
            _settingRepo = settingRepo;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            var defaultTopAmount = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("DefaultTopAmount").Value);
            var runListVM = new SpeedRunListViewModel(defaultTopAmount);

            return runListVM;
        }

        public IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID)
        {
            var allSpeedRunListCategories = _speedRunRepo.SpeedRunListCategories().ToList();
            var speedRunListCategories = allSpeedRunListCategories.Where(i => i.IsDefault).OrderBy(i => i.DefaultSortOrder).ToList();

            if(currUserAccountID > 0)
            {
                var userSpeedRunListCategories = _userAcctRepo.GetUserAccountSpeedRunListCategories(i => i.UserAccountID == currUserAccountID);
                if(userSpeedRunListCategories.Any())
                {
                    speedRunListCategories = (from c in speedRunListCategories
                                join uc in userSpeedRunListCategories
                                on c.ID equals uc.SpeedRunListCategoryID
                                orderby uc.ID
                                select c).ToList();
                }
            }
            
            return speedRunListCategories;
        }

        public IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset, categoryTypeID);
            IEnumerable<SpeedRunSummaryViewModel> runVMs = runs.Select(i => new SpeedRunSummaryViewModel(i));

            return runVMs;
        }

        public SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID)
        {
            var run = _speedRunRepo.GetSpeedRunSummaryViews(i => i.ID == speedRunID).FirstOrDefault();
            var runVM = new SpeedRunSummaryViewModel(run);

            return runVM;
        }

        public IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData)
        {
            var runs = new List<SpeedRunGridView>();
            if (showAllData) {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
            } else {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs && i.Rank.HasValue).OrderBy(i => i.PrimaryTime).ToList();
            }

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }

        public IEnumerable<WorldRecordGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID)
        {                                     
            var runs = _speedRunRepo.GetWorldRecordGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && (!categoryID.HasValue || i.CategoryID == categoryID) && (!levelID.HasValue || i.LevelID == levelID) && i.Rank == 1)
                                    .OrderBy(i => i.CategoryID)
                                    .ThenBy(i => i.LevelID)
                                    .ThenBy(i => i.SubCategoryVariableValueIDs)
                                    .ToList();
                                                                                                            
            var runVMs = runs.Select(i => new WorldRecordGridViewModel(i)).ToList();
            runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID && g.LevelID == i.LevelID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridUserViewModel> GetUserSpeedRunGridData(int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridUserViews(i => i.UserID == userID).ToList();            
            var runVMs = runs.Select(i => new SpeedRunGridUserViewModel(i)).ToList();
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
                            .ThenBy(i=>(i.DateSubmitted ?? i.VerifyDate ?? DateTime.MaxValue).Date)
                            .ThenBy(i=>(i.Rank ?? Int32.MaxValue))
                            .ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunChartViewModel> GetGameChartData(int gameID, int categoryTypeID)
        {
            var runs = _speedRunRepo.GetSpeedRunChartViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID)
                                    .OrderBy(i => i.CategoryID)
                                    .ThenBy(i => i.LevelID)
                                    .ThenBy(i => i.SubCategoryVariableValueIDs)
                                    .ToList();

            var runVMs = runs.Select(i => new SpeedRunChartViewModel(i)).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunChartViewModel> GetLeaderboardChartData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs)
        {
            var runs = _speedRunRepo.GetSpeedRunChartViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
            var runVMs = runs.Select(i => new SpeedRunChartViewModel(i)).ToList();

            return runVMs;
        }  

        public IEnumerable<SpeedRunChartViewModel> GetUserSpeedRunChartData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunChartUserViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs && i.UserID == userID).OrderByDescending(i => i.ID).ToList();     
            var runVMs = runs.Select(i => new SpeedRunChartViewModel((SpeedRunChartView)i)).ToList();

            return runVMs;
        }                      

        public ImportStatusViewModel GetImportStatus()
        {
            var importSettings = new List<string>() { "ImportLastRunDate", "ImportLastUpdateSpeedRunsDate", "ImportLastBulkReloadDate" };
            var results = _settingRepo.GetSettings(i => importSettings.Contains(i.Name)).ToList();
            var ImportLastRunDate = results.FirstOrDefault(i => i.Name == "ImportLastRunDate")?.Dte;
            var ImportLastUpdateSpeedRunsDate = results.FirstOrDefault(i => i.Name == "ImportLastUpdateSpeedRunsDate")?.Dte;
            var ImportLastBulkReloadDate = results.FirstOrDefault(i => i.Name == "ImportLastBulkReloadDate")?.Dte;

            var importStatusVM = new ImportStatusViewModel(ImportLastRunDate, ImportLastUpdateSpeedRunsDate, ImportLastBulkReloadDate);

            return importStatusVM;
        }        
    }
}
