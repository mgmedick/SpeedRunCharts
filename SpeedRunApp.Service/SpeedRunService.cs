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
            var runListVM = new SpeedRunListViewModel();

            return runListVM;
        }

        public IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID)
        {
            var speedRunListCategoryIDs = _userAcctRepo.GetUserAccountSpeedRunListCategories(i => i.UserAccountID == currUserAccountID).Select(i => i.SpeedRunListCategoryID);
            var speedRunListCategories = speedRunListCategoryIDs.Any() ? _speedRunRepo.SpeedRunListCategories(i => speedRunListCategoryIDs.Contains(i.ID)) : _speedRunRepo.SpeedRunListCategories(i => i.IsDefault).OrderBy(i => i.DefaultSortOrder);

            return speedRunListCategories;
        }

        public IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int? orderValueOffset)
        {
            var topAmount = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("DefaultSpeedRunListItemsPerPage").Value);
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
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


        //public IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID)
        //{
        //    var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryTypeID == categoryTypeID && i.Rank == 1).OrderBy(i => i.SubCategoryVariableValueIDs).ToList();
        //    var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
        //    runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

        //    return runVMs;
        //}

        //public IEnumerable<SpeedRunGridViewModel> GetPersonalBestGridData(int userID, int categoryTypeID)
        //{
        //    var runs = _speedRunRepo.GetPersonalBestsByUserID(userID, categoryTypeID);
        //    var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
        //    runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

        //    return runVMs;
        //}

        public IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryID, int? levelID, int? userID)
        {
            var runs = new List<SpeedRunGridView>();
            if (userID.HasValue)
            {
                runs = _speedRunRepo.GetPersonalBestsByUserID(gameID, categoryID, levelID, userID.Value).ToList();
            }
            else
            {
                runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.Rank == 1).OrderBy(i => i.SubCategoryVariableValueIDs).ToList();
            }

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            runVMs = runVMs.Where(i => i.SubCategoryVariableValueIDs?.Split(",").Count() == runVMs.Where(g => g.GameID == i.GameID && g.CategoryID == i.CategoryID && g.LevelID == i.LevelID).Select(h => h.SubCategoryVariableValueIDs?.Split(",").Count()).Max()).ToList();

            return runVMs;
        }

        public IEnumerable<SpeedRunGridViewModel> GetSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int? userID, bool showAllData)
        {
            var runs = new List<SpeedRunGridView>();
            if (userID.HasValue)
            {
                runs = _speedRunRepo.GetSpeedRunGridViewsByUserID(gameID, categoryID, levelID, subCategoryVariableValueIDs, userID.Value).ToList();
            }
            else
            {
                if (showAllData) {
                    runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs).OrderBy(i => i.PrimaryTime).ToList();
                } else {
                    runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.CategoryID == categoryID && i.LevelID == levelID && i.SubCategoryVariableValueIDs == subCategoryVariableValueIDs && i.Rank.HasValue).OrderBy(i => i.PrimaryTime).ToList();
                }
            }

            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();

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
