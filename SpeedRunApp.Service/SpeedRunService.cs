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
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ICacheService _cacheService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IUserAccountRepository _userAcctRepo = null;
        private readonly ISettingRepository _settingRepo = null;

        public SpeedRunService(IConfiguration config, IGameService gamesService, IUserService userService, ICacheService cacheService, ISpeedRunRepository speedRunRepo, IUserAccountRepository userAcctRepo, ISettingRepository settingRepo)
        {
            _config = config;
            _gamesService = gamesService;
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
            var userSpeedRunListCategories = _userAcctRepo.GetUserAccountSpeedRunListCategories(i => i.UserAccountID == currUserAccountID);
            var speedRunListCategories = _speedRunRepo.SpeedRunListCategories();
            
            if(userSpeedRunListCategories.Any())
            {
                speedRunListCategories = (from c in speedRunListCategories
                            join uc in userSpeedRunListCategories
                            on c.ID equals uc.SpeedRunListCategoryID
                            orderby uc.ID
                            select c).ToList();
            }
            else
            {
                speedRunListCategories = speedRunListCategories.Where(i => i.IsDefault).OrderBy(i => i.DefaultSortOrder).ToList();
            }

            return speedRunListCategories;
        }

        public IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset, categoryTypeID);
            IEnumerable<SpeedRunSummaryViewModel> runVMs = runs.Select(i => new SpeedRunSummaryViewModel(i));

            return runVMs;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(int gameID, int? speedRunID)
        {
            var gameVM = _gamesService.GetGame(gameID);
            var statusTypes = _cacheService.GetRunStatusTypes();
            SpeedRunViewModel runVM = null;
            if (speedRunID.HasValue)
            {
                var run = _speedRunRepo.GetSpeedRunViews(i => i.ID == speedRunID.Value).FirstOrDefault();
                runVM = new SpeedRunViewModel(run);
            }

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, runVM.Players, runVM.Guests, gameVM.CategoryTypes, gameVM.Categories, gameVM.Levels, gameVM.Platforms, gameVM.Variables, gameVM.SubCategoryVariables, runVM);

            return editSpeedRunVM;
        }

        public SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID)
        {
            var run = _speedRunRepo.GetSpeedRunSummaryViews(i => i.ID == speedRunID).FirstOrDefault();
            var runVM = new SpeedRunSummaryViewModel(run);

            return runVM;
        }

        public IEnumerable<SpeedRunGridViewModel> GetGameSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData)
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

        public IEnumerable<SpeedRunGridViewModel> GetUserSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViewsByUserID(gameID, categoryID, levelID, subCategoryVariableValueIDs, userID).ToList();      
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

            return runVMs;
        }

        public IEnumerable<WorldRecordGridViewModel> GetGameWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID)
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

        public IEnumerable<WorldRecordGridViewModel> GetPersonalBestGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID, int userID)
        {
           var runs = _speedRunRepo.GetPersonalBestsByUserID(gameID, categoryTypeID, categoryID, levelID, userID);           
           var runVMs = runs.Select(i => new WorldRecordGridViewModel(i)).ToList();
           runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

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
