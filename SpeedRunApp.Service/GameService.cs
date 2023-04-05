using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class GamesService : IGameService
    {
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IGameRepository _gameRepo = null;
        private readonly ICacheService _cacheService = null;
        private readonly ISettingRepository _settingRepo = null;
        private readonly ISpeedRunService _speedRunService = null;

        public GamesService(IGameRepository gameRepo, ISpeedRunRepository speedRunRepo, ICacheService cacheService, ISettingRepository settingRepo, ISpeedRunService speedRunService)
        {
            _gameRepo = gameRepo;
            _speedRunRepo = speedRunRepo;
            _cacheService = cacheService;
            _settingRepo = settingRepo;
            _speedRunService = speedRunService;
        }

        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunComID) {
            var gameVM = GetGame(gameAbbr);
            var speedRunID = string.IsNullOrWhiteSpace(speedRunComID) ? (int?)null : _speedRunRepo.GetSpeedRunID(speedRunComID);            
            var gameDetailsVM = new GameDetailsViewModel(gameVM, speedRunID);

            return gameDetailsVM;
        }

        public GameViewModel GetGame(string gameAbbr)
        {
            var game = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            var gameVM = game != null ? new GameViewModel(game) : null;

            return gameVM;
        }

        public GameViewModel GetGame(int gameID)
        {
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var gameVM = game != null ? new GameViewModel(game) : null;

            return gameVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        public EditSpeedRunViewModel GetEditSpeedRun(int gameID, int? speedRunID)
        {
            var gameVM = GetGame(gameID);
            var statusTypes = _cacheService.GetRunStatusTypes();
            SpeedRunViewModel runVM = null;
            if (speedRunID.HasValue)
            {
                var run = _speedRunRepo.GetSpeedRunViews(i => i.ID == speedRunID.Value).FirstOrDefault();
                runVM = new SpeedRunViewModel(run);
            }

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameVM.CategoryTypes, gameVM.Categories, gameVM.Levels, gameVM.Platforms, gameVM.Variables, gameVM.SubCategoryVariables, runVM);

            return editSpeedRunVM;
        }        

        public LeaderboardTabViewModel GetLeaderboardTabs(int gameID, int? speedRunID = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs).ToList();   
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridTabVM = new LeaderboardTabViewModel(tabItems, exportTypes);

            if (speedRunID.HasValue) {
                var run = runs.FirstOrDefault(i => i.ID == speedRunID);
                if (run == null) {
                    run = _speedRunRepo.GetSpeedRunGridTabViews(i => i.ID == speedRunID).FirstOrDefault();
                }

                if (run != null) {
                    var game = tabItems.FirstOrDefault(i => i.ID == run.GameID);
                    if (game != null) {
                        var categoryTypeID = game.Categories.Where(i=>i.ID == run.CategoryID).Select(i=>i.CategoryTypeID).FirstOrDefault();
                        var subCategoryVariableValueIDs = run.SubCategoryVariableValueIDs?.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                        var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(subCategoryVariableValueIDs, game.SubCategoryVariables);
                        var showAllData = !run.Rank.HasValue;

                        gridTabVM = new LeaderboardTabViewModel(tabItems, exportTypes, run.GameID, categoryTypeID, run.CategoryID, run.LevelID, subCategoryVariableValueNames, showAllData);
                    }
                }
            }

            return gridTabVM;
        }
        
        public WorldRecordTabViewModel GetWorldRecordTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs).ToList();   
            FilterGameTabsByHasData(tabItems, true);                             
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var tabVM = new WorldRecordTabViewModel(tabItems, exportTypes);

            return tabVM;
        }

        public UserSpeedRunTabViewModel GetUserSpeedRunTabsAndGridData(int userID, int? speedRunID = null)
        {
            var runVMs = _speedRunService.GetUserSpeedRunGridData(userID).ToList();            
            var runTabs = runVMs.Select(i=> new SpeedRunGridTabView() { ID = i.ID, GameID = i.GameID, CategoryID = i.CategoryID, LevelID = i.LevelID, SubCategoryVariableValueIDs = i.SubCategoryVariableValueIDs, Rank = i.Rank }).ToList();
            var gameIDs = runVMs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var tabItems = GetGameTabs(games, runTabs).ToList();
            FilterGameTabsByHasData(tabItems, true);
            var categoryTypes = tabItems.SelectMany(i=>i.CategoryTypes).GroupBy(g => new {g.ID}).Select(i=>i.First()).OrderBy(i=>i.ID).ToList();                                  
            var tabVM = new UserSpeedRunTabViewModel(tabItems, categoryTypes, runVMs);
                       
            return tabVM;
        }

        private IEnumerable<GameTabViewModel> GetGameTabs(IEnumerable<GameView> games, IEnumerable<SpeedRunGridTabView> runs = null)
        {
            var gameTabs = new List<GameTabViewModel>();

            foreach(var game in games)
            {
                var gameTab = new GameTabViewModel(game);
                SetGameTabHasData(gameTab, runs);
                gameTabs.Add(gameTab);
            }

            return gameTabs;
        }

        private void SetGameTabHasData(GameTabViewModel gameTab, IEnumerable<SpeedRunGridTabView> runs)
        {
            if (gameTab.Categories != null)
            {
                foreach(var category in gameTab.Categories)
                {
                    category.HasData = runs.Any(i => i.CategoryID == category.ID);

                    if (!category.HasData) {
                        category.Name += " (empty)";
                    }                    
                }
            }

            if (gameTab.CategoryTypes != null)
            {
                var categoryTypeIDsToRemove = new List<int>();
                foreach (var categoryType in gameTab.CategoryTypes)
                {                    
                    if (!gameTab.Categories.Any(i => i.CategoryTypeID == categoryType.ID && i.HasData))
                    {
                        categoryTypeIDsToRemove.Add(categoryType.ID);
                    }
                }

                gameTab.CategoryTypes.RemoveAll(i => categoryTypeIDsToRemove.Contains(i.ID));
            }

            if (gameTab.Levels != null)
            {
                foreach (var level in gameTab.Levels)
                {
                    level.HasData = runs.Any(i => i.CategoryID == level.CategoryID && i.LevelID == level.ID);

                    if (!level.HasData) {
                        level.Name += " (empty)";
                    }
                }
            }

            if (gameTab.SubCategoryVariablesTabs != null)
            {
                SetGameTabVariablesHasValue(gameTab.SubCategoryVariablesTabs, gameTab.SubCategoryVariablesTabs, runs.ToList());
            }
        }

        private void SetGameTabVariablesHasValue(List<Variable> allVariables, List<Variable> variables, List<SpeedRunGridTabView> runs, string parentVariableValues = null)
        {
           foreach (var variable in variables)
           {
                foreach (var variableValue in variable.VariableValues)
                {
                    var variableValues = string.IsNullOrWhiteSpace(parentVariableValues) ? variableValue.ID.ToString() : parentVariableValues + "," + variableValue.ID.ToString();                                                            
                    variableValue.HasData = runs.Any(i => i.CategoryID == variable.CategoryID
                                        && i.LevelID == variable.LevelID
                                        && !string.IsNullOrWhiteSpace(i.SubCategoryVariableValueIDs)
                                        && i.SubCategoryVariableValueIDs.StartsWith(variableValues));

                    if (!variableValue.HasData) {
                        variableValue.Name += " (empty)";
                    }

                    var subvars = allVariables.Where(i => i.CategoryID == variable.CategoryID && i.LevelID == variable.LevelID).ToList();
                    foreach(var subvar in subvars)
                    {
                        foreach(var va in subvar.VariableValues)
                        {
                            if (va.ID == variableValue.ID)
                            {
                                va.HasData = variableValue.HasData;
                            }
                        }
                    }

                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        SetGameTabVariablesHasValue(allVariables, variableValue.SubVariables.ToList(), runs, variableValues);
                    }
                }

                parentVariableValues = null;    
           }
        }        

        private void FilterGameTabsByHasData(List<GameTabViewModel> tabItems, bool hasData)
        {
            foreach (var tabItem in tabItems) {
                tabItem.Categories = tabItem.Categories?.Where(i => i.HasData == hasData).ToList();
                tabItem.Levels = tabItem.Levels?.Where(i => i.HasData == hasData).ToList();

                if(tabItem.SubCategoryVariablesTabs != null && tabItem.SubCategoryVariablesTabs.Any())
                {
                    FilterGameTabSubCategoryVariablesByHasData(tabItem.SubCategoryVariablesTabs, hasData);
                }
            }   
        }

        private void FilterGameTabSubCategoryVariablesByHasData(List<Variable> variables, bool hasData)
        {
            foreach(var variable in variables){
                variable.VariableValues = variable.VariableValues.Where(i => i.HasData == hasData).ToList();
                
                foreach (var variableValue in variable.VariableValues) {
                    if(variableValue.SubVariables != null && variableValue.SubVariables.Any()) {
                        FilterGameTabSubCategoryVariablesByHasData(variableValue.SubVariables.ToList(), hasData);
                    }
                }      
            } 
        }

        private Dictionary<string, string> GetSubCategoryVariableValueNames(List<int> runSubCategoryVariableValueIDs, List<Variable> gameSubCategoryVariables)
        {                
            var SubCategoryVariableValueNames = new Dictionary<string, string>();

            var variableCount = 0;
            if (runSubCategoryVariableValueIDs != null) {
                foreach (var runSubCategoryVariableValueID in runSubCategoryVariableValueIDs) {
                    var variable = gameSubCategoryVariables.FirstOrDefault(i => i.VariableValues.Any(g => g.ID == runSubCategoryVariableValueID));
                    var variableValue = variable?.VariableValues?.FirstOrDefault(i => i.ID == runSubCategoryVariableValueID);
                    
                    if (variable != null && variableValue != null) {
                        SubCategoryVariableValueNames.Add(variable.Name + variableCount, variableValue.Name);
                    }

                    variableCount++;
                }
            }

            return SubCategoryVariableValueNames;
        }

        public List<string> SetGameIsChanged(int gameID)
        {
            var errorMessages = new List<string>();
            var isBulkReloadRunning = _settingRepo.GetSetting("IsBulkReloadRunning")?.Num == 1;
            
            if (isBulkReloadRunning)
            {
                errorMessages.Add("Import is running Bulk Reload, Games cannot be updated until complete");
            }
            else
            {
                var game = _gameRepo.GetGames(i => i.ID == gameID).FirstOrDefault();
                if (game != null) {
                    if (game.IsChanged.HasValue && game.IsChanged.Value){
                        errorMessages.Add("Game is alreay updating");
                    } else {
                        game.IsChanged = true;
                        _gameRepo.UpdateGameIsChanged(game);
                    }
                }
            }

            return errorMessages;
        }       
    }
}

