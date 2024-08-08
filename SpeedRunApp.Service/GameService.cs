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

        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunCode) {
            var gameVW = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            var gameDetailsVM = new GameDetailsViewModel(gameVW, speedRunCode);

            return gameDetailsVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        /*
        public EditSpeedRunViewModel GetEditSpeedRun(int gameID, int? speedRunID)
        {
            var gameVW = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
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
        */

        public LeaderboardTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs).ToList();   
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridTabVM = new LeaderboardTabViewModel(tabItems, exportTypes);

            if (!string.IsNullOrWhiteSpace(speedRunCode)) {
                var run = runs.FirstOrDefault(i => i.Code == speedRunCode);
                if (run == null) {
                    run = _speedRunRepo.GetSpeedRunGridTabViews(i => i.Code == speedRunCode).FirstOrDefault();
                }

                if (run != null) {
                    var game = tabItems.FirstOrDefault(i => i.ID == run.GameID);
                    if (game != null) {
                        var category = game.Categories.FirstOrDefault(i=>i.ID == run.CategoryID);
                        var categoryTypeID = category != null ? category.CategoryTypeID : 0;
                        var subCategoryVariableValueIDs = run.SubCategoryVariableValueIDs?.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                        var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(subCategoryVariableValueIDs, game.SubCategoryVariables);
                        var showAllData = !run.Rank.HasValue;
                        var showMisc = category != null ? category.IsMiscellaneous : false;

                        gridTabVM = new LeaderboardTabViewModel(tabItems, exportTypes, run.GameID, categoryTypeID, run.CategoryID, run.LevelID, subCategoryVariableValueNames, showAllData, showMisc);
                    }
                }
            }

            return gridTabVM;
        }
        
        public LeaderboardTabViewModel GetWorldRecordTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs, true).ToList();   
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var tabVM = new LeaderboardTabViewModel(tabItems, exportTypes);

            return tabVM;
        }

        public LeaderboardTabViewModel GetGameChartTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs, true).ToList();   
            var tabVM = new LeaderboardTabViewModel(tabItems);

            return tabVM;
        }        

        public PlayerSpeedRunTabViewModel GetPlayerSpeedRunTabsAndData(int playerID)
        {
            var runVMs = _speedRunService.GetPlayerSpeedRunGridData(playerID).ToList();            
            var runTabs = runVMs.Select(i=> new SpeedRunGridTabView() { ID = i.ID, GameID = i.GameID, CategoryID = i.CategoryID, LevelID = i.LevelID, SubCategoryVariableValueIDs = i.SubCategoryVariableValueIDs, Rank = i.Rank }).ToList();
            var gameIDs = runVMs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var tabItems = GetGameTabs(games, runTabs, true).ToList();
            var categoryTypes = tabItems.SelectMany(i=>i.CategoryTypes).GroupBy(g => new {g.ID}).Select(i=>i.First()).OrderBy(i=>i.ID).ToList();                                  
            var tabVM = new PlayerSpeedRunTabViewModel(tabItems, categoryTypes, runVMs);
                       
            return tabVM;
        }
        
        private IEnumerable<GameTabViewModel> GetGameTabs(IEnumerable<GameView> games, IEnumerable<SpeedRunGridTabView> runs = null, bool hasDataOnly = false)
        {
            var gameTabs = new List<GameTabViewModel>();

            foreach(var game in games)
            {
                var gameTab = new GameTabViewModel(game);
                SetGameTabHasData(gameTab, runs);
                if (gameTab.SubCategoryVariablesTabs != null)
                {
                    gameTab.SubCategoryVariablesTabs = FilterGameTabSubCategoryVariablesByHasData(gameTab.SubCategoryVariablesTabs, true);
                }

                if (hasDataOnly)
                {
                    FilterGameTabByHasData(gameTab, true);
                }

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

        private void FilterGameTabByHasData(GameTabViewModel tabItem, bool hasData)
        {
            tabItem.Categories = tabItem.Categories?.Where(i => i.HasData == hasData).ToList();
            tabItem.Levels = tabItem.Levels?.Where(i => i.HasData == hasData).ToList();

            if(tabItem.SubCategoryVariablesTabs != null && tabItem.SubCategoryVariablesTabs.Any())
            {
                tabItem.SubCategoryVariablesTabs = FilterGameTabSubCategoryVariablesByHasData(tabItem.SubCategoryVariablesTabs, hasData);
                FilterGameTabSubCategoryVariableValuesByHasData(tabItem.SubCategoryVariablesTabs, hasData);
            }
        }

        private List<Variable> FilterGameTabSubCategoryVariablesByHasData(List<Variable> variables, bool hasData)
        {
            variables = variables.Where(x => x.HasData == hasData).ToList();
 
            foreach (var variable in variables)
            {
                foreach (var variableValue in variable.VariableValues)
                {
                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        variableValue.SubVariables = FilterGameTabSubCategoryVariablesByHasData(variableValue.SubVariables.ToList(), hasData);
                    }                    
                }
            }

            return variables;
        }

        private void FilterGameTabSubCategoryVariableValuesByHasData(List<Variable> variables, bool hasData)
        {
            foreach(var variable in variables) {
                variable.VariableValues = variable.VariableValues.Where(i => i.HasData == hasData).ToList();

                foreach (var variableValue in variable.VariableValues) {
                    if(variableValue.SubVariables != null && variableValue.SubVariables.Any()) {
                        FilterGameTabSubCategoryVariableValuesByHasData(variableValue.SubVariables.ToList(), hasData);
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
    }
}

